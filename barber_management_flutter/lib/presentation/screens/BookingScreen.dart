import 'package:flutter/foundation.dart';
import 'package:flutter/material.dart';
import 'package:barbermanagemobile/data/models/promotion_model.dart';
import 'package:barbermanagemobile/domain/entities/booking_create_order.dart';
import 'package:barbermanagemobile/domain/usecases/create_booking_order_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/get_all_branches_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/get_booking_service_detail_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/get_employees_by_branch_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/get_employees_by_date_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/get_customer_by_user_id_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/get_promotions_use_case.dart';
import 'package:barbermanagemobile/presentation/providers/auth_provider.dart';
import 'package:get_it/get_it.dart';
import 'package:intl/intl.dart';
import 'package:provider/provider.dart';

class BookingScreen extends StatefulWidget {
  final VoidCallback onBack;

  const BookingScreen({super.key, required this.onBack});

  @override
  _BookingScreenState createState() => _BookingScreenState();
}

class _BookingScreenState extends State<BookingScreen> {
  final GetAllBranchesUseCase _getAllBranchesUseCase = GetIt.instance<GetAllBranchesUseCase>();
  final GetEmployeesByBranchUseCase _getEmployeesByBranchUseCase = GetIt.instance<GetEmployeesByBranchUseCase>();
  final GetEmployeesByDateUseCase _getEmployeesByDateUseCase = GetIt.instance<GetEmployeesByDateUseCase>();
  final GetBookingServiceDetailUseCase _getBookingServiceDetailUseCase = GetIt.instance<GetBookingServiceDetailUseCase>();
  final CreateBookingOrderUseCase _createBookingOrderUseCase = GetIt.instance<CreateBookingOrderUseCase>();
  final GetCustomerByUserIDUseCase _getCustomerByUserIDUseCase = GetIt.instance<GetCustomerByUserIDUseCase>();
  final GetPromotionsUseCase _getPromotionsUseCase = GetIt.instance<GetPromotionsUseCase>();

  List<Map<String, dynamic>> branches = [];
  List<Map<String, dynamic>> employees = [];
  List<Map<String, dynamic>> availableEmployeesByDate = [];
  List<Map<String, dynamic>> serviceDetails = [];
  List<PromotionModel> promotions = [];
  PromotionModel? selectedPromotion;

  String? selectedBranch;
  String? selectedEmployee;
  DateTime? selectedDateTime;
  String? selectedServiceDetail; // Stores serviceDetailID
  int? custID;
  bool isLoading = false;
  String? errorMessage;

  static const primaryColor = Color(0xFF4E342E);
  static const backgroundColor = Color(0xFF212121);
  static const textColor = Color(0xFFEFEBE9);
  static const accentColor = Color(0xFF8D6E63);
  static const dropdownTextColor = Color(0xFFD7CCC8);

  final int defaultServiceID = 1; // Hardcoded serviceID; adjust or make dynamic if needed

  @override
  void initState() {
    super.initState();
    _loadCustomerID();
    _loadBranches();
    _loadPromotions();
  }

  Future<void> _loadCustomerID() async {
    final authProvider = Provider.of<AuthProvider>(context, listen: false);
    final user = authProvider.user;
    if (user == null) {
      setState(() {
        errorMessage = 'Vui lòng đăng nhập để đặt lịch';
      });
      if (kDebugMode) {
        print('No user logged in');
      }
      return;
    }

    final userId = user.userId;
    if ( userId.isEmpty || int.tryParse(userId) == null) {
      setState(() {
        errorMessage = 'Thông tin người dùng không hợp lệ (userId không khả dụng)';
      });
      if (kDebugMode) {
        print('Invalid userId: $userId');
      }
      return;
    }

    final userIdInt = int.parse(userId);
    if (kDebugMode) {
      print('Loading customer ID for userId: $userIdInt');
    }

    setState(() => isLoading = true);
    try {
      final result = await _getCustomerByUserIDUseCase.call(userIdInt);
      result.fold(
        (error) => throw Exception(error),
        (customer) => setState(() {
          custID = customer.customerID;
          isLoading = false;
        }),
      );
    } catch (e) {
      setState(() {
        isLoading = false;
        errorMessage = 'Lỗi khi lấy thông tin khách hàng: $e';
      });
      if (mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text('Lỗi khi lấy thông tin khách hàng: $e'),
            backgroundColor: primaryColor,
          ),
        );
      }
    }
  }

  Future<void> _loadBranches() async {
    setState(() => isLoading = true);
    try {
      branches = await _getAllBranchesUseCase.call();
      setState(() => isLoading = false);
    } catch (e) {
      setState(() {
        isLoading = false;
        errorMessage = 'Lỗi khi lấy danh sách chi nhánh: $e';
      });
      if (mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(content: Text('Lỗi khi lấy danh sách chi nhánh: $e'), backgroundColor: primaryColor),
        );
      }
    }
  }

  Future<void> _loadEmployeesByBranch(int branchID) async {
    setState(() => isLoading = true);
    try {
      employees = await _getEmployeesByBranchUseCase.call(branchID);
      print('Loaded employees for branch $branchID: $employees');
      setState(() => isLoading = false);
    } catch (e) {
      setState(() {
        isLoading = false;
        errorMessage = 'Lỗi khi lấy danh sách nhân viên: $e';
      });
      if (mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(content: Text('Lỗi khi lấy danh sách nhân viên: $e'), backgroundColor: primaryColor),
        );
      }
    }
  }

  Future<void> _loadEmployeesByDate(DateTime date, int branchID) async {
    setState(() => isLoading = true);
    try {
      availableEmployeesByDate = await _getEmployeesByDateUseCase.call(date, branchID);
      print('Loaded employees by date $date, branch $branchID: $availableEmployeesByDate');
      setState(() => isLoading = false);
    } catch (e) {
      setState(() {
        isLoading = false;
        errorMessage = 'Lỗi khi lấy nhân viên theo ngày: $e';
      });
      if (mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(content: Text('Lỗi khi lấy nhân viên theo ngày: $e'), backgroundColor: primaryColor),
        );
      }
    }
  }

  Future<void> _loadServiceDetails() async {
    setState(() => isLoading = true);
    try {
      serviceDetails = await _getBookingServiceDetailUseCase.call(defaultServiceID);
      print('Loaded service details for service $defaultServiceID: $serviceDetails');
      setState(() => isLoading = false);
    } catch (e) {
      setState(() {
        isLoading = false;
        errorMessage = 'Lỗi khi lấy chi tiết dịch vụ: $e';
      });
      if (mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(content: Text('Lỗi khi lấy chi tiết dịch vụ: $e'), backgroundColor: primaryColor),
        );
      }
    }
  }

  Future<void> _loadPromotions() async {
    setState(() => isLoading = true);
    try {
      final fetchedPromotions = await _getPromotionsUseCase.call();
      promotions = [
        PromotionModel(
          promoID: 0,
          promoName: 'Không áp dụng',
          promoDescription: '',
          promoDiscount: 0.0,
          promoImage: '',
        ),
        ...fetchedPromotions,
      ];
      print('Loaded promotions: $promotions');
      setState(() => isLoading = false);
    } catch (e) {
      setState(() {
        isLoading = false;
        errorMessage = 'Lỗi khi lấy danh sách khuyến mãi: $e';
      });
      if (mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(content: Text('Lỗi khi lấy danh sách khuyến mãi: $e'), backgroundColor: primaryColor),
        );
      }
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: backgroundColor,
      appBar: AppBar(
        elevation: 0,
        backgroundColor: primaryColor,
        title: Text(
          "Đặt lịch giữ chỗ",
          style: TextStyle(
            fontFamily: 'Poppins',
            fontWeight: FontWeight.w700,
            fontSize: 24,
            color: textColor,
          ),
        ),
        leading: IconButton(
          icon: Icon(Icons.arrow_back, color: textColor),
          onPressed: widget.onBack,
        ),
      ),
      body: isLoading
          ? Center(child: CircularProgressIndicator(color: accentColor))
          : SingleChildScrollView(
              padding: EdgeInsets.symmetric(horizontal: 16, vertical: 20),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  _buildStep(
                    stepNumber: 1,
                    title: "Chọn salon",
                    isActive: true,
                    child: _buildSelectionButton(
                      icon: Icons.store,
                      text: selectedBranch == null
                          ? "Xem tất cả salon"
                          : branches.firstWhere((b) => b['branchID'].toString() == selectedBranch)['branchName'],
                      onTap: () async {
                        await _loadBranches();
                        if (mounted) {
                          showModalBottomSheet(
                            context: context,
                            builder: (context) => Container(
                              color: primaryColor.withOpacity(0.9),
                              child: ListView(
                                children: branches.map((branch) {
                                  return ListTile(
                                    leading: Icon(Icons.store, color: Colors.red),
                                    title: Text(branch['branchName'], style: TextStyle(color: dropdownTextColor)),
                                    onTap: () {
                                      setState(() {
                                        selectedBranch = branch['branchID'].toString();
                                        selectedEmployee = null;
                                        selectedDateTime = null;
                                        selectedServiceDetail = null;
                                        selectedPromotion = null;
                                        _loadEmployeesByBranch(int.parse(selectedBranch!));
                                      });
                                      Navigator.pop(context);
                                    },
                                  );
                                }).toList(),
                              ),
                            ),
                          );
                        }
                      },
                      highlightText: selectedBranch != null ? "Tìm salon gần anh" : null,
                      highlightColor: accentColor,
                    ),
                  ),
                  _buildStep(
                    stepNumber: 2,
                    title: "Chọn dịch vụ chi tiết",
                    isActive: selectedBranch != null,
                    child: _buildSelectionButton(
                      icon: Icons.cut,
                      text: selectedServiceDetail == null
                          ? "Xem tất cả dịch vụ chi tiết"
                          : serviceDetails.firstWhere((s) => s['servID'].toString() == selectedServiceDetail)['servName'],
                      onTap: selectedBranch != null
                          ? () async {
                              await _loadServiceDetails();
                              if (mounted) {
                                showModalBottomSheet(
                                  context: context,
                                  builder: (context) => Container(
                                    color: primaryColor.withOpacity(0.9),
                                    child: ListView(
                                      children: serviceDetails.map((serviceDetail) {
                                        return ListTile(
                                          leading: Icon(Icons.cut, color: textColor),
                                          title: Text(
                                            "${serviceDetail['servName']} - ${NumberFormat.currency(locale: 'vi_VN', symbol: '₫').format(serviceDetail['servPrice'])}",
                                            style: TextStyle(color: dropdownTextColor),
                                          ),
                                          onTap: () {
                                            setState(() {
                                              selectedServiceDetail = serviceDetail['servID'].toString();
                                            });
                                            Navigator.pop(context);
                                          },
                                        );
                                      }).toList(),
                                    ),
                                  ),
                                );
                              }
                            }
                          : null,
                    ),
                  ),
                  _buildStep(
                    stepNumber: 3,
                    title: "Chọn ngày, giờ & stylist",
                    isActive: selectedServiceDetail != null,
                    child: Column(
                      children: [
                        _buildSelectionButton(
                          icon: Icons.calendar_today,
                          text: selectedDateTime == null
                              ? "Hôm nay, ${DateFormat('EEE (dd/MM)').format(DateTime.now())}"
                              : DateFormat('Hôm nay, EEE (dd/MM)').format(selectedDateTime!),
                          onTap: selectedServiceDetail != null
                              ? () async {
                                  final date = await showDatePicker(
                                    context: context,
                                    initialDate: DateTime.now(),
                                    firstDate: DateTime.now(),
                                    lastDate: DateTime.now().add(Duration(days: 30)),
                                    builder: (context, child) {
                                      return Theme(
                                        data: ThemeData.dark().copyWith(
                                          colorScheme: ColorScheme.dark(
                                            primary: accentColor,
                                            onPrimary: textColor,
                                            surface: primaryColor.withOpacity(0.9),
                                            onSurface: dropdownTextColor,
                                          ),
                                        ),
                                        child: child!,
                                      );
                                    },
                                  );
                                  if (date != null) {
                                    final time = await showTimePicker(
                                      context: context,
                                      initialTime: TimeOfDay.now(),
                                      builder: (context, child) {
                                        return Theme(
                                          data: ThemeData.dark().copyWith(
                                            colorScheme: ColorScheme.dark(
                                              primary: accentColor,
                                              onPrimary: textColor,
                                              surface: primaryColor.withOpacity(0.9),
                                              onSurface: dropdownTextColor,
                                            ),
                                          ),
                                          child: child!,
                                        );
                                      },
                                    );
                                    if (time != null && mounted) {
                                      setState(() {
                                        selectedDateTime = DateTime(date.year, date.month, date.day, time.hour, time.minute);
                                        if (selectedBranch != null) {
                                          _loadEmployeesByDate(selectedDateTime!, int.parse(selectedBranch!));
                                        }
                                      });
                                    }
                                  }
                                }
                              : null,
                          highlightText: selectedDateTime != null ? "Ngày thường" : null,
                          highlightColor: Colors.green,
                        ),
                        if (selectedDateTime != null && availableEmployeesByDate.isNotEmpty)
                          Padding(
                            padding: const EdgeInsets.only(top: 8),
                            child: _buildSelectionButton(
                              icon: Icons.person,
                              text: selectedEmployee == null
                                  ? "Chọn stylist"
                                  : availableEmployeesByDate.firstWhere((e) => e['empID'] == selectedEmployee)['empName'],
                              onTap: () async {
                                showModalBottomSheet(
                                  context: context,
                                  builder: (context) => Container(
                                    color: primaryColor.withOpacity(0.9),
                                    child: ListView(
                                      children: availableEmployeesByDate.map((employee) {
                                        return ListTile(
                                          leading: Icon(Icons.person, color: textColor),
                                          title: Text(
                                            "${employee['empName']} (${employee['position']})",
                                            style: TextStyle(color: dropdownTextColor),
                                          ),
                                          onTap: () {
                                            setState(() {
                                              selectedEmployee = employee['empID'];
                                            });
                                            Navigator.pop(context);
                                          },
                                        );
                                      }).toList(),
                                    ),
                                  ),
                                );
                              },
                            ),
                          ),
                      ],
                    ),
                  ),
                  _buildStep(
                    stepNumber: 4,
                    title: "Chọn mã giảm giá",
                    isActive: selectedEmployee != null,
                    child: _buildSelectionButton(
                      icon: Icons.local_offer,
                      text: selectedPromotion == null
                          ? "Chọn mã giảm giá"
                          : "${selectedPromotion!.promoName} (${(selectedPromotion!.promoDiscount * 100).toInt()}%)",
                      onTap: selectedEmployee != null
                          ? () async {
                              showModalBottomSheet(
                                context: context,
                                builder: (context) => Container(
                                  color: primaryColor.withOpacity(0.9),
                                  child: ListView(
                                    children: promotions.map((promo) {
                                      return ListTile(
                                        leading: Icon(Icons.local_offer, color: textColor),
                                        title: Text(
                                          "${promo.promoName} (${(promo.promoDiscount * 100).toInt()}%)",
                                          style: TextStyle(color: dropdownTextColor),
                                        ),
                                        subtitle: Text(
                                          promo.promoDescription,
                                          style: TextStyle(color: dropdownTextColor.withOpacity(0.7)),
                                        ),
                                        onTap: () {
                                          setState(() {
                                            selectedPromotion = promo;
                                          });
                                          Navigator.pop(context);
                                        },
                                      );
                                    }).toList(),
                                  ),
                                ),
                              );
                            }
                          : null,
                    ),
                  ),
                  if (_isFormValid())
                    Padding(
                      padding: const EdgeInsets.symmetric(vertical: 16),
                      child: Container(
                        padding: EdgeInsets.all(12),
                        decoration: BoxDecoration(
                          color: primaryColor.withOpacity(0.2),
                          borderRadius: BorderRadius.circular(8),
                        ),
                        child: Text(
                          "Cắt xong trả tiền, hủy lịch không sao",
                          style: TextStyle(color: textColor, fontSize: 14),
                        ),
                      ),
                    ),
                  if (_isFormValid())
                    SizedBox(
                      width: double.infinity,
                      child: ElevatedButton(
                        onPressed: () => _showConfirmationDialog(context),
                        style: ElevatedButton.styleFrom(
                          backgroundColor: accentColor,
                          padding: EdgeInsets.symmetric(vertical: 16),
                          shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(8)),
                          elevation: 0,
                        ),
                        child: Text(
                          "Chốt giờ cắt",
                          style: TextStyle(
                            fontFamily: 'Poppins',
                            fontSize: 16,
                            fontWeight: FontWeight.w600,
                            color: textColor,
                          ),
                        ),
                      ),
                    ),
                  if (errorMessage != null)
                    Padding(
                      padding: const EdgeInsets.only(top: 16),
                      child: Text(
                        errorMessage!,
                        style: TextStyle(color: Colors.red, fontSize: 14),
                      ),
                    ),
                ],
              ),
            ),
    );
  }

  bool _isFormValid() {
    return selectedBranch != null &&
        selectedDateTime != null &&
        selectedServiceDetail != null &&
        selectedEmployee != null &&
        custID != null;
  }

  void _showConfirmationDialog(BuildContext context) {
    final selectedBranchData = branches.firstWhere((b) => b['branchID'].toString() == selectedBranch);
    final selectedServiceDetailData = serviceDetails.firstWhere((s) => s['servID'].toString() == selectedServiceDetail);
    final selectedEmployeeData = availableEmployeesByDate.firstWhere((e) => e['empID'] == selectedEmployee);

    showDialog(
      context: context,
      builder: (context) => AlertDialog(
        shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(20)),
        backgroundColor: backgroundColor,
        title: Text(
          "Xác Nhận Đặt Lịch",
          style: TextStyle(fontFamily: 'Poppins', fontWeight: FontWeight.w700, color: textColor),
        ),
        content: Column(
          mainAxisSize: MainAxisSize.min,
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            _buildConfirmationRow("Cơ sở", selectedBranchData['branchName']),
            _buildConfirmationRow("Thời gian", DateFormat('dd/MM/yyyy - HH:mm').format(selectedDateTime!)),
            _buildConfirmationRow("Dịch vụ chi tiết", selectedServiceDetailData['servName']),
            _buildConfirmationRow("Stylist", selectedEmployeeData['empName']),
            if (selectedPromotion != null && selectedPromotion!.promoID != 0)
              _buildConfirmationRow(
                "Mã giảm giá",
                "${selectedPromotion!.promoName} (${(selectedPromotion!.promoDiscount * 100).toInt()}%)",
              ),
          ],
        ),
        actions: [
          TextButton(
            onPressed: () => Navigator.pop(context),
            child: Text("Hủy", style: TextStyle(color: Colors.grey)),
          ),
          Builder(
            builder: (dialogContext) => ElevatedButton(
              onPressed: () async {
                try {
                  final bookingOrder = BookingCreateOrder(
                    appoint: [
                      BookingCreateBusinessAppoint(
                        servID: int.parse(selectedServiceDetail!), // Use serviceDetailID
                        empID: int.parse(selectedEmployee!),
                        appStatus: "OK",
                      ),
                    ],
                    order: BookingCreateOrderBusiness(
                      custID: custID!,
                      createAt: DateTime.now(),
                      orderDate: selectedDateTime!,
                    ),
                    promoID: selectedPromotion?.promoID ?? 0,
                  );

                  setState(() => isLoading = true);
                  await _createBookingOrderUseCase.call(bookingOrder);
                  setState(() {
                    isLoading = false;
                    errorMessage = null;
                  });

                  Navigator.pop(dialogContext);
                  ScaffoldMessenger.of(dialogContext).showSnackBar(
                    SnackBar(
                      content: Text("Đặt lịch thành công!", style: TextStyle(color: textColor)),
                      backgroundColor: primaryColor,
                      behavior: SnackBarBehavior.floating,
                      shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(12)),
                    ),
                  );
                  widget.onBack();
                } catch (e) {
                  setState(() {
                    isLoading = false;
                    errorMessage = "Lỗi đặt lịch: $e";
                  });
                }
              },
              style: ElevatedButton.styleFrom(
                backgroundColor: accentColor,
                shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(10)),
              ),
              child: Text("Xác nhận", style: TextStyle(color: textColor)),
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildStep({required int stepNumber, required String title, required bool isActive, required Widget child}) {
    return Row(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Column(
          children: [
            Container(
              width: 24,
              height: 24,
              decoration: BoxDecoration(
                shape: BoxShape.circle,
                color: isActive ? primaryColor : Colors.grey.shade300,
              ),
              child: Center(
                child: Text(
                  stepNumber.toString(),
                  style: TextStyle(
                    color: textColor,
                    fontWeight: FontWeight.bold,
                    fontSize: 14,
                  ),
                ),
              ),
            ),
            if (stepNumber < 4)
              Container(
                width: 2,
                height: 60,
                color: isActive ? Colors.grey.shade300 : Colors.grey.shade200,
              ),
          ],
        ),
        SizedBox(width: 16),
        Expanded(
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Text(
                "$stepNumber. $title",
                style: TextStyle(
                  fontSize: 18,
                  fontWeight: FontWeight.w600,
                  color: textColor,
                  fontFamily: 'Poppins',
                ),
              ),
              SizedBox(height: 8),
              child,
            ],
          ),
        ),
      ],
    );
  }

  Widget _buildSelectionButton({
    required IconData icon,
    required String text,
    required Future<void> Function()? onTap,
    String? highlightText,
    Color? highlightColor,
  }) {
    return GestureDetector(
      onTap: onTap != null ? () async => await onTap() : null,
      child: Container(
        padding: EdgeInsets.symmetric(vertical: 12, horizontal: 16),
        decoration: BoxDecoration(
          color: primaryColor.withOpacity(0.2),
          borderRadius: BorderRadius.circular(8),
        ),
        child: Row(
          children: [
            Icon(icon, color: onTap != null ? textColor : Colors.grey),
            SizedBox(width: 12),
            Expanded(
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Text(
                    text,
                    style: TextStyle(
                      fontFamily: 'Poppins',
                      color: onTap != null ? dropdownTextColor : Colors.grey,
                      fontSize: 14,
                    ),
                  ),
                  if (highlightText != null)
                    Padding(
                      padding: const EdgeInsets.only(top: 4),
                      child: Container(
                        padding: EdgeInsets.symmetric(horizontal: 8, vertical: 4),
                        decoration: BoxDecoration(
                          color: highlightColor ?? accentColor,
                          borderRadius: BorderRadius.circular(4),
                        ),
                        child: Text(
                          highlightText,
                          style: TextStyle(
                            color: textColor,
                            fontSize: 12,
                          ),
                        ),
                      ),
                    ),
                ],
              ),
            ),
            Icon(Icons.chevron_right, color: onTap != null ? textColor : Colors.grey),
          ],
        ),
      ),
    );
  }

  Widget _buildConfirmationRow(String label, String value) {
    return Padding(
      padding: EdgeInsets.symmetric(vertical: 4),
      child: Row(
        children: [
          Text("$label: ", style: TextStyle(fontWeight: FontWeight.w600, color: textColor)),
          Expanded(child: Text(value, style: TextStyle(color: dropdownTextColor))),
        ],
      ),
    );
  }
}