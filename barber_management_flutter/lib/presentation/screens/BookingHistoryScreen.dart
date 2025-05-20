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
import 'package:barbermanagemobile/domain/usecases/get_employee_by_id_use_case.dart';
import 'package:url_launcher/url_launcher.dart';

class BookingHistoryScreen extends StatefulWidget {
  final int custID;

  const BookingHistoryScreen({super.key, required this.custID});

  @override
  _BookingHistoryScreenState createState() => _BookingHistoryScreenState();
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
  final GetEmployeeByIdUseCase _getEmployeeByIdUseCase =
      GetIt.instance<GetEmployeeByIdUseCase>();

  List<BookingOrder> orders = [];
  bool isLoading = false;
  String? errorMessage;

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
      // Fetch invoice details
      final invoice = await _getInvoiceByOrderIdUseCase.call(orderID);
      final appointments = invoice['order']?['appointments'] as List<dynamic>? ?? [];

      print('Fetching details for appointments: $appointments');

      // Fetch service and employee details for each appointment
      final details = <Map<String, dynamic>>[];
      for (var appt in appointments) {
        final serviceID = int.tryParse(appt['service']?.toString() ?? '');
        final empID = int.tryParse(appt['empID']?.toString() ?? '');

        Map<String, dynamic> serviceDetail = {};
        Map<String, dynamic> employeeDetail = {};

        if (serviceID != null) {
          try {
            final serviceDetails = await _getBookingServiceDetailUseCase.call(serviceID);
            serviceDetail = serviceDetails.isNotEmpty ? serviceDetails[0] : {};
          } catch (e) {
            print('Error fetching service details for serviceID $serviceID: $e');
            serviceDetail = {'servName': 'Không lấy được thông tin dịch vụ', 'servPrice': 0.0};
          }
        } else {
          serviceDetail = {'servName': 'Không có dịch vụ', 'servPrice': 0.0};
        }

        if (empID != null) {
          try {
            employeeDetail = await _getEmployeeByIdUseCase.call(empID);
          } catch (e) {
            print('Error fetching employee details for empID $empID: $e');
            employeeDetail = {'empName': 'Không lấy được thông tin nhân viên'};
          }
        } else {
          employeeDetail = {'empName': 'Không có nhân viên'};
        }

        details.add({
          'service': serviceDetail,
          'employee': employeeDetail,
          'appointmentStatus': appt['appointmentStatus']?.toString() ?? 'Unknown',
        });
      }

      setState(() => isLoading = false);

      // Show modal with invoice details
      if (mounted) {
        await showDialog(
          context: context,
          builder: (context) => AlertDialog(
            backgroundColor: primaryColor,
            shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(20)),
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
                  _buildDetailRow('Mã hóa đơn', invoice['invoiceID']?.toString() ?? 'N/A'),
                  _buildDetailRow(
                    'Ngày lập hóa đơn',
                    DateFormat('dd/MM/yyyy HH:mm').format(
                      DateTime.tryParse(invoice['invoiceDate']?.toString() ?? '') ?? DateTime.now(),
                    ),
                  ),
                  _buildDetailRow('Tình trạng', invoice['status']?.toString() ?? 'N/A'),
                  _buildDetailRow(
                    'Tổng tiền',
                    NumberFormat.currency(locale: 'vi_VN', symbol: '₫').format(invoice['totalAmount'] ?? 0.0),
                  ),
                  _buildDetailRow('Phương thức thanh toán', invoice['paymentMethod']?.toString() ?? 'N/A'),
                  SizedBox(height: 16),
                  Text(
                    'Dịch vụ và Nhân viên',
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
                      'Không có thông tin dịch vụ hoặc nhân viên',
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
                            _buildDetailRow('Dịch vụ', detail['service']?['servName']?.toString() ?? 'N/A'),
                            _buildDetailRow(
                              'Giá dịch vụ',
                              NumberFormat.currency(locale: 'vi_VN', symbol: '₫')
                                  .format(detail['service']?['servPrice'] ?? 0.0),
                            ),
                            _buildDetailRow('Nhân viên', detail['employee']?['empName']?.toString() ?? 'N/A'),
                            _buildDetailRow('Trạng thái lịch hẹn', detail['appointmentStatus']?.toString() ?? 'N/A'),
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
                    invoice['paymentTerms']?.toString() ?? 'Không có điều khoản',
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
              : ListView.builder(
                  padding: EdgeInsets.symmetric(horizontal: 16, vertical: 20),
                  itemCount: orders.length,
                  itemBuilder: (context, index) {
                    final order = orders[index];
                    return _buildOrderItem(order);
                  },
                ),
    );
  }

  Widget _buildOrderItem(BookingOrder order) {
    final statusInfo = _getStatusInfo(order.orderStatus);

    return Card(
      margin: EdgeInsets.symmetric(vertical: 10),
      color: primaryColor.withOpacity(0.3),
      shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(16)),
      elevation: 4,
      child: Padding(
        padding: EdgeInsets.all(20),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Row(
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(
                  "Đơn hàng #${order.orderID}",
                  style: TextStyle(
                    fontSize: 20,
                    fontWeight: FontWeight.w700,
                    color: textColor,
                    fontFamily: 'Poppins',
                  ),
                ),
                Container(
                  padding: EdgeInsets.symmetric(horizontal: 10, vertical: 6),
                  decoration: BoxDecoration(
                    color: statusInfo['color'],
                    borderRadius: BorderRadius.circular(8),
                  ),
                  child: Text(
                    statusInfo['text'],
                    style: TextStyle(
                      color: textColor,
                      fontSize: 13,
                      fontWeight: FontWeight.w600,
                      fontFamily: 'Poppins',
                    ),
                  ),
                ),
              ],
            ),
            SizedBox(height: 12),
            _buildOrderDetailRow(
              "Tổng tiền",
              NumberFormat.currency(locale: 'vi_VN', symbol: '₫').format(order.orderTotal),
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
        return {
          'text': 'Hoàn thành',
          'color': Colors.green,
        };
      case 'UnConfirm':
        return {
          'text': 'Chưa xác nhận',
          'color': Colors.orange,
        };
      case 'Cancelled':
        return {
          'text': 'Đã hủy',
          'color': Colors.red,
        };
      default:
        return {
          'text': 'Không xác định',
          'color': Colors.grey,
        };
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
              final confirm = await _showDeleteConfirmationDialog(order.orderID);
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
              final confirm = await _showPaymentConfirmationDialog(order.orderID);
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
              shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(10)),
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
              shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(10)),
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