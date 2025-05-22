import 'package:flutter/foundation.dart' show kIsWeb;
import 'package:flutter/material.dart';
import 'package:get_it/get_it.dart';
import 'package:intl/intl.dart';
import 'package:barbermanagemobile/domain/entities/booking_order.dart';
import 'package:barbermanagemobile/domain/usecases/delete_booking_order_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/get_orders_by_customer_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/pay_booking_order_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/get_invoice_by_order_id_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/get_booking_service_detail_use_case.dart';
import 'package:url_launcher/url_launcher.dart';

class BookingHistoryScreen extends StatefulWidget {
  final int custID;

  const BookingHistoryScreen({super.key, required this.custID});

  @override
  State<BookingHistoryScreen> createState() => _BookingHistoryScreenState();
}

class _BookingHistoryScreenState extends State<BookingHistoryScreen> {
  final GetOrdersByCustomerUseCase _getOrdersByCustomerUseCase =
      GetIt.instance<GetOrdersByCustomerUseCase>();
  final DeleteBookingOrderUseCase _deleteBookingOrderUseCase =
      GetIt.instance<DeleteBookingOrderUseCase>();
  final PayBookingOrderUseCase _payBookingOrderUseCase =
      GetIt.instance<PayBookingOrderUseCase>();
  final GetInvoiceByOrderIdUseCase _getInvoiceByOrderIdUseCase =
      GetIt.instance<GetInvoiceByOrderIdUseCase>();
  final GetBookingServiceDetailUseCase _getBookingServiceDetailUseCase =
      GetIt.instance<GetBookingServiceDetailUseCase>();

  List<BookingOrder> orders = [];
  bool isLoading = false;
  String? errorMessage;
  DateTime? selectedDate;

  static const primaryColor = Color(0xFF4E342E);
  static const backgroundColor = Color(0xFF212121);
  static const textColor = Color(0xFFEFEBE9);
  static const accentColor = Color(0xFF8D6E63);
  static const dropdownTextColor = Color(0xFFD7CCC8);

  @override
  void initState() {
    super.initState();
    _loadOrders();
  }

  Future<void> _loadOrders() async {
    setState(() => isLoading = true);
    try {
      orders = await _getOrdersByCustomerUseCase.call(widget.custID);
      print('Loaded orders: ${orders.map((o) => o.orderID).toList()}');
      if (orders.isNotEmpty) {
        final datesWithOrders = _getDatesWithOrders();
        if (datesWithOrders.isNotEmpty) {
          selectedDate = datesWithOrders.first;
        }
      }
      setState(() => isLoading = false);
    } catch (e) {
      setState(() {
        isLoading = false;
        errorMessage = 'Error loading orders: $e';
      });
      if (mounted) {
        ScaffoldMessenger.of(context)
            .showSnackBar(SnackBar(content: Text('Error loading orders: $e')));
      }
    }
  }

  List<DateTime> _getDatesWithOrders() {
    final uniqueDates = <DateTime>{};
    for (var order in orders) {
      final orderDate = DateTime(
        order.orderDate.year,
        order.orderDate.month,
        order.orderDate.day,
      );
      uniqueDates.add(orderDate);
    }
    return uniqueDates.toList()..sort((a, b) => b.compareTo(a));
  }

  Future<void> _deleteOrder(int orderID) async {
    try {
      await _deleteBookingOrderUseCase.call(orderID);
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(
          content: Text('Xóa đơn hàng thành công'),
          backgroundColor: Colors.green,
        ),
      );
      await _loadOrders();
    } catch (e) {
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(
          content: Text('Xóa đơn hàng thất bại: $e'),
          backgroundColor: Colors.red,
        ),
      );
    }
  }

  Future<void> _payOrder(int orderID) async {
    try {
      print("check1 >>>>>> $orderID");
      final paymentUrl = await _payBookingOrderUseCase.call(orderID);
      print("check2 >>>>>> $paymentUrl");

      final Uri uri = Uri.parse(paymentUrl);

      if (kIsWeb) {
        if (!await launchUrl(uri, webOnlyWindowName: '_self')) {
          throw Exception('Không thể mở URL thanh toán trên web');
        }
      } else {
        if (!await launchUrl(uri, mode: LaunchMode.externalApplication)) {
          throw Exception('Không thể mở URL thanh toán trên thiết bị');
        }
      }
    } catch (e) {
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(
          content: Text('Lỗi khi khởi tạo thanh toán: $e'),
          backgroundColor: Colors.red,
        ),
      );
    }
  }

  Future<void> _showInvoiceDetails(int orderID) async {
    setState(() => isLoading = true);
    try {
      final invoice = await _getInvoiceByOrderIdUseCase.call(orderID);
      final appointments =
          invoice['order']?['appointments'] as List<dynamic>? ?? [];

      print('Fetching details for appointments: $appointments');

      final details = <Map<String, dynamic>>[];
      for (var appt in appointments) {
        final serviceID = int.tryParse(appt['service']?.toString() ?? '');

        Map<String, dynamic> serviceDetail = {};

        if (serviceID != null) {
          try {
            final serviceDetails =
                await _getBookingServiceDetailUseCase.call(serviceID);
            serviceDetail = serviceDetails.isNotEmpty ? serviceDetails[0] : {};
          } catch (e) {
            print(
                'Error fetching service details for serviceID $serviceID: $e');
            serviceDetail = {
              'servName': 'Không lấy được thông tin dịch vụ',
              'servPrice': 0.0
            };
          }
        } else {
          serviceDetail = {'servName': 'Không có dịch vụ', 'servPrice': 0.0};
        }

        details.add({
          'service': serviceDetail,
          'appointmentStatus':
              appt['appointmentStatus']?.toString() ?? 'Unknown',
        });
      }

      setState(() => isLoading = false);

      if (mounted) {
        await showDialog(
          context: context,
          builder: (context) => AlertDialog(
            backgroundColor: primaryColor,
            shape:
                RoundedRectangleBorder(borderRadius: BorderRadius.circular(20)),
            title: Text(
              'Chi tiết hóa đơn #$orderID',
              style: TextStyle(
                color: textColor,
                fontFamily: 'Poppins',
                fontWeight: FontWeight.w700,
              ),
            ),
            content: SingleChildScrollView(
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                mainAxisSize: MainAxisSize.min,
                children: [
                  _buildDetailRow(
                      'Mã hóa đơn', invoice['invoiceID']?.toString() ?? 'N/A'),
                  _buildDetailRow(
                    'Ngày lập hóa đơn',
                    DateFormat('dd/MM/yyyy HH:mm').format(
                      DateTime.tryParse(
                              invoice['invoiceDate']?.toString() ?? '') ??
                          DateTime.now(),
                    ),
                  ),
                  _buildDetailRow(
                      'Tình trạng', invoice['status']?.toString() ?? 'N/A'),
                  _buildDetailRow(
                    'Tổng tiền',
                    NumberFormat.currency(locale: 'vi_VN', symbol: '₫')
                        .format(invoice['totalAmount']/100 ?? 0.0),
                  ),
                  _buildDetailRow('Phương thức thanh toán',
                      invoice['paymentMethod']?.toString() ?? 'N/A'),
                  SizedBox(height: 16),
                  Text(
                    'Dịch vụ',
                    style: TextStyle(
                      color: textColor,
                      fontFamily: 'Poppins',
                      fontWeight: FontWeight.w600,
                      fontSize: 16,
                    ),
                  ),
                  SizedBox(height: 8),
                  if (details.isEmpty)
                    Text(
                      'Không có thông tin dịch vụ',
                      style: TextStyle(
                        color: dropdownTextColor,
                        fontFamily: 'Poppins',
                        fontSize: 14,
                      ),
                    )
                  else
                    ...details.map((detail) => Column(
                          crossAxisAlignment: CrossAxisAlignment.start,
                          children: [
                            _buildDetailRow(
                                'Dịch vụ',
                                detail['service']?['servName']?.toString() ??
                                    'N/A'),
                            _buildDetailRow(
                              'Giá dịch vụ',
                              NumberFormat.currency(
                                      locale: 'vi_VN', symbol: '₫')
                                  .format(
                                      detail['service']?['servPrice'] ?? 0.0),
                            ),
                            _buildDetailRow(
                                'Trạng thái lịch hẹn',
                                detail['appointmentStatus']?.toString() ??
                                    'N/A'),
                            SizedBox(height: 8),
                          ],
                        )),
                  SizedBox(height: 16),
                  Text(
                    'Điều khoản thanh toán',
                    style: TextStyle(
                      color: textColor,
                      fontFamily: 'Poppins',
                      fontWeight: FontWeight.w600,
                      fontSize: 16,
                    ),
                  ),
                  SizedBox(height: 8),
                  Text(
                    invoice['paymentTerms']?.toString() ??
                        'Không có điều khoản',
                    style: TextStyle(
                      color: dropdownTextColor,
                      fontFamily: 'Poppins',
                      fontSize: 14,
                    ),
                  ),
                ],
              ),
            ),
            actions: [
              TextButton(
                onPressed: () => Navigator.pop(context),
                child: Text(
                  'Đóng',
                  style: TextStyle(
                    color: accentColor,
                    fontFamily: 'Poppins',
                  ),
                ),
              ),
            ],
          ),
        );
      }
    } catch (e) {
      setState(() {
        isLoading = false;
        errorMessage = 'Lỗi khi lấy chi tiết hóa đơn: $e';
      });
      if (mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text('Lỗi khi lấy chi tiết hóa đơn: $e'),
            backgroundColor: Colors.red,
          ),
        );
      }
    }
  }

  Future<bool?> _showDeleteConfirmationDialog(int orderID) async {
    return showDialog<bool>(
      context: context,
      builder: (context) => AlertDialog(
        backgroundColor: primaryColor,
        title: Text(
          'Xác nhận xóa',
          style: TextStyle(
            color: textColor,
            fontFamily: 'Poppins',
            fontWeight: FontWeight.w700,
          ),
        ),
        content: Text(
          'Bạn có chắc chắn muốn xóa đơn hàng #$orderID không?',
          style: TextStyle(
            color: dropdownTextColor,
            fontFamily: 'Poppins',
          ),
        ),
        actions: [
          TextButton(
            onPressed: () => Navigator.pop(context, false),
            child: Text(
              'Hủy',
              style: TextStyle(
                color: accentColor,
                fontFamily: 'Poppins',
              ),
            ),
          ),
          TextButton(
            onPressed: () => Navigator.pop(context, true),
            child: Text(
              'Xóa',
              style: TextStyle(
                color: Colors.red,
                fontFamily: 'Poppins',
              ),
            ),
          ),
        ],
      ),
    );
  }

  Future<bool?> _showPaymentConfirmationDialog(int orderID) async {
    return showDialog<bool>(
      context: context,
      builder: (context) => AlertDialog(
        backgroundColor: primaryColor,
        title: Text(
          'Xác nhận thanh toán',
          style: TextStyle(
            color: textColor,
            fontFamily: 'Poppins',
            fontWeight: FontWeight.w700,
          ),
        ),
        content: Text(
          'Bạn có chắc chắn muốn thanh toán đơn hàng #$orderID không?',
          style: TextStyle(
            color: dropdownTextColor,
            fontFamily: 'Poppins',
          ),
        ),
        actions: [
          TextButton(
            onPressed: () => Navigator.pop(context, false),
            child: Text(
              'Hủy',
              style: TextStyle(
                color: accentColor,
                fontFamily: 'Poppins',
              ),
            ),
          ),
          TextButton(
            onPressed: () => Navigator.pop(context, true),
            child: Text(
              'Thanh toán',
              style: TextStyle(
                color: Colors.green,
                fontFamily: 'Poppins',
              ),
            ),
          ),
        ],
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: backgroundColor,
      appBar: AppBar(
        elevation: 0,
        backgroundColor: primaryColor,
        title: Text(
          "Lịch Sử Đặt Lịch",
          style: TextStyle(
            fontFamily: 'Poppins',
            fontWeight: FontWeight.w700,
            fontSize: 24,
            color: dropdownTextColor,
          ),
        ),
        leading: IconButton(
          icon: Icon(Icons.arrow_back, color: dropdownTextColor),
          onPressed: () => Navigator.pop(context),
        ),
      ),
      body: isLoading
          ? Center(child: CircularProgressIndicator(color: accentColor))
          : orders.isEmpty
              ? Center(
                  child: Text(
                    "Chưa có đơn hàng nào",
                    style: TextStyle(
                      fontSize: 18,
                      color: dropdownTextColor,
                      fontFamily: 'Poppins',
                    ),
                  ),
                )
              : Column(
                  children: [
                    _buildDatePicker(),
                    Expanded(child: _buildDailyGroupedOrders()),
                  ],
                ),
    );
  }

  Widget _buildDatePicker() {
    final datesWithOrders = _getDatesWithOrders();

    if (datesWithOrders.isEmpty) {
      return SizedBox.shrink();
    }

    return Container(
      height: 80,
      color: primaryColor.withOpacity(0.1),
      padding: EdgeInsets.symmetric(vertical: 8),
      child: ListView.builder(
        scrollDirection: Axis.horizontal,
        itemCount: datesWithOrders.length,
        itemBuilder: (context, index) {
          final date = datesWithOrders[index];
          final isSelected = selectedDate != null &&
              selectedDate!.day == date.day &&
              selectedDate!.month == date.month &&
              selectedDate!.year == date.year;

          return GestureDetector(
            onTap: () {
              setState(() {
                selectedDate = date;
              });
            },
            child: Container(
              width: 60,
              margin: EdgeInsets.symmetric(horizontal: 4),
              decoration: BoxDecoration(
                color: isSelected ? accentColor : primaryColor.withOpacity(0.3),
                borderRadius: BorderRadius.circular(12),
                border: Border.all(
                  color: isSelected ? accentColor : Colors.transparent,
                  width: 2,
                ),
              ),
              child: Column(
                mainAxisAlignment: MainAxisAlignment.center,
                children: [
                  Text(
                    DateFormat('dd').format(date),
                    style: TextStyle(
                      fontSize: 18,
                      fontWeight: FontWeight.w700,
                      color: isSelected ? textColor : dropdownTextColor,
                      fontFamily: 'Poppins',
                    ),
                  ),
                  Text(
                    DateFormat('EEE').format(date),
                    style: TextStyle(
                      fontSize: 12,
                      color: isSelected ? textColor : dropdownTextColor,
                      fontFamily: 'Poppins',
                    ),
                  ),
                ],
              ),
            ),
          );
        },
      ),
    );
  }

  Widget _buildDailyGroupedOrders() {
    if (selectedDate == null) {
      return Center(
        child: Text(
          'Không có đơn hàng nào',
          style: TextStyle(
            fontSize: 16,
            color: dropdownTextColor,
            fontFamily: 'Poppins',
          ),
        ),
      );
    }

    final dailyOrders = orders
        .where((order) =>
            order.orderDate.day == selectedDate!.day &&
            order.orderDate.month == selectedDate!.month &&
            order.orderDate.year == selectedDate!.year)
        .toList();

    final morningOrders = dailyOrders
        .where(
            (order) => order.orderDate.hour >= 7 && order.orderDate.hour < 12)
        .toList()
      ..sort((a, b) => a.orderDate.compareTo(b.orderDate));
    final afternoonOrders = dailyOrders
        .where(
            (order) => order.orderDate.hour >= 12 && order.orderDate.hour < 17)
        .toList()
      ..sort((a, b) => a.orderDate.compareTo(b.orderDate));
    final eveningOrders = dailyOrders
        .where(
            (order) => order.orderDate.hour >= 17 && order.orderDate.hour <= 21)
        .toList()
      ..sort((a, b) => a.orderDate.compareTo(b.orderDate));

    return ListView(
      padding: EdgeInsets.symmetric(horizontal: 16, vertical: 20),
      children: [
        if (morningOrders.isNotEmpty)
          _buildShiftSection(
              'Ca Sáng (7h - 12h)', morningOrders, Icons.wb_sunny),
        if (afternoonOrders.isNotEmpty)
          _buildShiftSection(
              'Ca Chiều (12h - 17h)', afternoonOrders, Icons.wb_twighlight),
        if (eveningOrders.isNotEmpty)
          _buildShiftSection(
              'Ca Tối (17h - 21h)', eveningOrders, Icons.nightlight_round),
        if (morningOrders.isEmpty &&
            afternoonOrders.isEmpty &&
            eveningOrders.isEmpty)
          Padding(
            padding: const EdgeInsets.all(16),
            child: Text(
              'Không có đơn hàng trong ngày này',
              style: TextStyle(
                fontSize: 16,
                color: dropdownTextColor,
                fontFamily: 'Poppins',
              ),
            ),
          ),
      ],
    );
  }

  Widget _buildShiftSection(
      String shiftTitle, List<BookingOrder> shiftOrders, IconData icon) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Padding(
          padding: const EdgeInsets.fromLTRB(16, 16, 16, 8),
          child: Row(
            children: [
              Icon(icon, color: accentColor, size: 24),
              SizedBox(width: 8),
              Text(
                shiftTitle,
                style: TextStyle(
                  fontSize: 16,
                  fontWeight: FontWeight.w600,
                  color: textColor,
                  fontFamily: 'Poppins',
                ),
              ),
            ],
          ),
        ),
        ...shiftOrders.map((order) => Padding(
              padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 8),
              child: _buildOrderItem(order),
            )),
      ],
    );
  }

  Widget _buildOrderItem(BookingOrder order) {
    final statusInfo = _getStatusInfo(order.orderStatus);

    return Card(
      margin: EdgeInsets.symmetric(vertical: 0),
      color: primaryColor.withOpacity(0.3),
      shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(16)),
      elevation: 4,
      child: Padding(
        padding: EdgeInsets.all(16),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Row(
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Flexible(
                  child: Text(
                    "Đơn hàng #${order.orderID}",
                    style: TextStyle(
                      fontSize: 20,
                      fontWeight: FontWeight.w700,
                      color: textColor,
                      fontFamily: 'Poppins',
                    ),
                    overflow: TextOverflow.ellipsis,
                  ),
                ),
                Flexible(
                  child: Container(
                    padding: EdgeInsets.symmetric(horizontal: 8, vertical: 4),
                    decoration: BoxDecoration(
                      color: statusInfo['color'],
                      borderRadius: BorderRadius.circular(6),
                    ),
                    child: Text(
                      statusInfo['text'],
                      style: TextStyle(
                        fontSize: 11,
                        fontWeight: FontWeight.w600,
                        color: textColor,
                        fontFamily: 'Poppins',
                      ),
                      textAlign: TextAlign.center,
                      overflow: TextOverflow.ellipsis,
                    ),
                  ),
                ),
              ],
            ),
            SizedBox(height: 12),
            _buildOrderDetailRow(
              "Tổng tiền",
              NumberFormat.currency(locale: 'vi_VN', symbol: '₫')
                  .format(order.orderTotal),
            ),
            _buildOrderDetailRow(
              "Ngày đặt",
              DateFormat('dd/MM/yyyy HH:mm').format(order.orderDate),
            ),
            _buildOrderDetailRow(
              "Tạo lúc",
              DateFormat('dd/MM/yyyy HH:mm').format(order.createAt),
            ),
            SizedBox(height: 16),
            Row(
              mainAxisAlignment: MainAxisAlignment.spaceEvenly,
              children: _buildActionButtons(order),
            ),
          ],
        ),
      ),
    );
  }

  Map<String, dynamic> _getStatusInfo(String status) {
    switch (status) {
      case 'Completed':
        return {'text': 'Hoàn thành', 'color': Colors.green};
      case 'UnConfirm':
        return {'text': 'Chưa xác nhận', 'color': Colors.orange};
      case 'Cancelled':
        return {'text': 'Đã hủy', 'color': Colors.red};
      default:
        return {'text': 'Không xác định', 'color': Colors.grey};
    }
  }

  List<Widget> _buildActionButtons(BookingOrder order) {
    switch (order.orderStatus) {
      case 'Cancelled':
        return [
          _buildButton(
            text: 'Xóa',
            icon: Icons.delete,
            color: Colors.red,
            isFilled: false,
            onPressed: () async {
              final confirm =
                  await _showDeleteConfirmationDialog(order.orderID);
              if (confirm == true) {
                await _deleteOrder(order.orderID);
              }
            },
          ),
        ];
      case 'UnConfirm':
        return [
          _buildButton(
            text: 'Thanh toán',
            icon: Icons.payment,
            color: Colors.green,
            isFilled: true,
            onPressed: () async {
              final confirm =
                  await _showPaymentConfirmationDialog(order.orderID);
              if (confirm == true) {
                await _payOrder(order.orderID);
              }
            },
          ),
          _buildButton(
            text: 'Hủy',
            icon: Icons.cancel,
            color: Colors.red,
            isFilled: false,
            onPressed: () {
              // TODO: Implement cancel action
            },
          ),
        ];
      case 'Completed':
        return [
          _buildButton(
            text: 'Chi tiết',
            icon: Icons.info,
            color: accentColor,
            isFilled: true,
            onPressed: () => _showInvoiceDetails(order.orderID),
          ),
        ];
      default:
        return [];
    }
  }

  Widget _buildButton({
    required String text,
    required IconData icon,
    required Color color,
    required bool isFilled,
    required VoidCallback onPressed,
  }) {
    return isFilled
        ? ElevatedButton.icon(
            onPressed: onPressed,
            style: ElevatedButton.styleFrom(
              backgroundColor: color,
              padding: EdgeInsets.symmetric(horizontal: 14, vertical: 10),
              shape: RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(10)),
              elevation: 0,
              minimumSize: Size(100, 40),
            ),
            icon: Icon(icon, size: 18, color: textColor),
            label: Text(
              text,
              style: TextStyle(
                fontSize: 13,
                fontWeight: FontWeight.w600,
                color: textColor,
                fontFamily: 'Poppins',
              ),
            ),
          )
        : OutlinedButton.icon(
            onPressed: onPressed,
            style: OutlinedButton.styleFrom(
              side: BorderSide(color: color, width: 1.5),
              padding: EdgeInsets.symmetric(horizontal: 14, vertical: 10),
              shape: RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(10)),
              minimumSize: Size(100, 40),
            ),
            icon: Icon(icon, size: 18, color: color),
            label: Text(
              text,
              style: TextStyle(
                fontSize: 13,
                fontWeight: FontWeight.w600,
                color: color,
                fontFamily: 'Poppins',
              ),
            ),
          );
  }

  Widget _buildOrderDetailRow(String label, String value) {
    return Padding(
      padding: EdgeInsets.symmetric(vertical: 6),
      child: Row(
        children: [
          Text(
            "$label: ",
            style: TextStyle(
              fontSize: 14,
              fontWeight: FontWeight.w600,
              color: textColor,
              fontFamily: 'Poppins',
            ),
          ),
          Expanded(
            child: Text(
              value,
              style: TextStyle(
                fontSize: 14,
                color: dropdownTextColor,
                fontFamily: 'Poppins',
              ),
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildDetailRow(String label, String value) {
    return Padding(
      padding: EdgeInsets.symmetric(vertical: 4),
      child: Row(
        children: [
          Text(
            '$label: ',
            style: TextStyle(
              fontSize: 14,
              fontWeight: FontWeight.w600,
              color: textColor,
              fontFamily: 'Poppins',
            ),
          ),
          Expanded(
            child: Text(
              value,
              style: TextStyle(
                fontSize: 14,
                color: dropdownTextColor,
                fontFamily: 'Poppins',
              ),
            ),
          ),
        ],
      ),
    );
  }
}