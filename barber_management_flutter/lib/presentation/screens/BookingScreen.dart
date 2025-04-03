import 'package:barbermanagemobile/domain/entities/booking_create_order.dart';
import 'package:barbermanagemobile/domain/usecases/create_booking_order_use_case.dart';
import 'package:flutter/material.dart';
import 'package:get_it/get_it.dart';
import 'package:intl/intl.dart';
import 'dart:async';
import 'package:barbermanagemobile/domain/repositories/booking_service_repository.dart';

class BookingScreen extends StatefulWidget {
  @override
  _BookingScreenState createState() => _BookingScreenState();
}

class _BookingScreenState extends State<BookingScreen> {
  final BookingServiceRepository _repository = GetIt.instance<BookingServiceRepository>();
  final CreateBookingOrderUseCase _createBookingOrderUseCase = GetIt.instance<CreateBookingOrderUseCase>();

  List<Map<String, dynamic>> branches = [];
  List<Map<String, dynamic>> employees = [];
  List<Map<String, dynamic>> availableEmployeesByDate = [];
  List<Map<String, dynamic>> services = [];
  final List<String> discountCodes = ['GIAM10', 'GIAM20', 'FREESHIP'];

  String? selectedBranch;
  String? selectedEmployee;
  DateTime? selectedDateTime;
  String? selectedService;
  String? selectedDiscountCode;

  bool isLoading = false;

  static const primaryColor = Color(0xFF4E342E); // Nâu đậm
  static const backgroundColor = Color(0xFF212121); // Đen xám
  static const textColor = Color(0xFFEFEBE9); // Xám nhạt
  static const accentColor = Color(0xFF8D6E63); // Nâu nhạt
  static const dropdownTextColor = Color(0xFFD7CCC8); // Nâu xám nhạt

  @override
  void initState() {
    super.initState();
    _loadBranches();
  }

  Future<void> _loadBranches() async {
    setState(() => isLoading = true);
    try {
      branches = await _repository.getAllBranches();
      setState(() => isLoading = false);
    } catch (e) {
      setState(() => isLoading = false);
      ScaffoldMessenger.of(context).showSnackBar(SnackBar(content: Text('Error loading branches: $e')));
    }
  }

  Future<void> _loadEmployeesByBranch(int branchID) async {
    setState(() => isLoading = true);
    try {
      employees = await _repository.getEmployeesByBranch(branchID);
      setState(() => isLoading = false);
    } catch (e) {
      setState(() => isLoading = false);
      ScaffoldMessenger.of(context).showSnackBar(SnackBar(content: Text('Error loading employees: $e')));
    }
  }

  Future<void> _loadEmployeesByDate(DateTime date) async {
    setState(() => isLoading = true);
    try {
      availableEmployeesByDate = await _repository.getEmployeesByDate(date);
      setState(() => isLoading = false);
    } catch (e) {
      setState(() => isLoading = false);
      ScaffoldMessenger.of(context).showSnackBar(SnackBar(content: Text('Error loading employees by date: $e')));
    }
  }

  Future<void> _loadServices() async {
    setState(() => isLoading = true);
    try {
      services = await _repository.getAllServices();
      setState(() => isLoading = false);
    } catch (e) {
      setState(() => isLoading = false);
      ScaffoldMessenger.of(context).showSnackBar(SnackBar(content: Text('Error loading services: $e')));
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
          "Đặt Lịch",
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
          : SingleChildScrollView(
              padding: EdgeInsets.all(20),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  _buildSectionTitle("Chọn Cơ Sở"),
                  DropdownButtonFormField<String>(
                    value: selectedBranch,
                    decoration: _inputDecoration("Cơ sở"),
                    dropdownColor: primaryColor.withOpacity(0.9),
                    items: branches.map((branch) {
                      return DropdownMenuItem<String>(
                        value: branch['branchID'].toString(),
                        child: Text(branch['branchName'], style: TextStyle(fontFamily: 'Poppins', color: dropdownTextColor)),
                      );
                    }).toList(),
                    onChanged: (value) {
                      setState(() {
                        selectedBranch = value;
                        selectedEmployee = null;
                        selectedDateTime = null;
                        selectedService = null;
                        selectedDiscountCode = null;
                        _loadEmployeesByBranch(int.parse(value!));
                      });
                    },
                  ),
                  SizedBox(height: 20),
                  if (selectedBranch != null) ...[
                    _buildSectionTitle("Chọn Thời Gian"),
                    GestureDetector(
                      onTap: () async {
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
                                  onPrimary: dropdownTextColor,
                                  surface: primaryColor.withOpacity(0.9),
                                  onSurface: dropdownTextColor,
                                ),
                                dialogBackgroundColor: backgroundColor,
                                textTheme: TextTheme(bodyMedium: TextStyle(color: dropdownTextColor)),
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
                                    onPrimary: dropdownTextColor,
                                    surface: primaryColor.withOpacity(0.9),
                                    onSurface: dropdownTextColor,
                                  ),
                                  dialogBackgroundColor: backgroundColor,
                                  textTheme: TextTheme(bodyMedium: TextStyle(color: dropdownTextColor)),
                                ),
                                child: child!,
                              );
                            },
                          );
                          if (time != null) {
                            setState(() {
                              selectedDateTime = DateTime(date.year, date.month, date.day, time.hour, time.minute);
                              _loadEmployeesByDate(selectedDateTime!);
                            });
                          }
                        }
                      },
                      child: Container(
                        padding: EdgeInsets.symmetric(vertical: 16, horizontal: 12),
                        decoration: BoxDecoration(
                          border: Border.all(color: Colors.grey[600]!),
                          borderRadius: BorderRadius.circular(12),
                          color: primaryColor.withOpacity(0.2),
                        ),
                        child: Row(
                          mainAxisAlignment: MainAxisAlignment.spaceBetween,
                          children: [
                            Text(
                              selectedDateTime != null
                                  ? DateFormat('dd/MM/yyyy - HH:mm').format(selectedDateTime!)
                                  : "Chọn ngày và giờ",
                              style: TextStyle(
                                fontFamily: 'Poppins',
                                color: selectedDateTime != null ? textColor : Colors.grey[400],
                                fontSize: 16,
                              ),
                            ),
                            Icon(Icons.calendar_today, color: accentColor),
                          ],
                        ),
                      ),
                    ),
                    SizedBox(height: 20),
                  ],
                  if (selectedDateTime != null && employees.isNotEmpty) ...[
                    _buildSectionTitle("Chọn Nhân Viên"),
                    DropdownButtonFormField<String>(
                      value: selectedEmployee,
                      decoration: _inputDecoration("Nhân viên"),
                      dropdownColor: primaryColor.withOpacity(0.9),
                      items: employees.map((emp) {
                        final isAvailable = availableEmployeesByDate.any((e) => e['employeeId'] == emp['empID']);
                        return DropdownMenuItem<String>(
                          value: emp['empID'].toString(),
                          child: Text(
                            emp['authUser']?['fullName'] ?? 'Unknown',
                            style: TextStyle(
                              fontFamily: 'Poppins',
                              color: isAvailable ? Colors.greenAccent : Colors.grey,
                            ),
                          ),
                        );
                      }).toList(),
                      onChanged: (value) {
                        setState(() {
                          selectedEmployee = value;
                          _loadServices();
                        });
                      },
                    ),
                    SizedBox(height: 20),
                  ],
                  if (selectedEmployee != null && services.isNotEmpty) ...[
                    _buildSectionTitle("Chọn Dịch Vụ"),
                    DropdownButtonFormField<String>(
                      value: selectedService,
                      decoration: _inputDecoration("Dịch vụ"),
                      dropdownColor: primaryColor.withOpacity(0.9),
                      items: services.map((service) {
                        return DropdownMenuItem<String>(
                          value: service['servID'].toString(),
                          child: Text(
                            "${service['servName']} - ${NumberFormat.currency(locale: 'vi_VN', symbol: '₫').format(service['servPrice'])}",
                            style: TextStyle(fontFamily: 'Poppins', color: dropdownTextColor),
                          ),
                        );
                      }).toList(),
                      onChanged: (value) {
                        setState(() {
                          selectedService = value;
                        });
                      },
                    ),
                    SizedBox(height: 20),
                  ],
                  if (selectedService != null) ...[
                    _buildSectionTitle("Chọn Mã Giảm Giá"),
                    DropdownButtonFormField<String>(
                      value: selectedDiscountCode,
                      decoration: _inputDecoration("Mã giảm giá (nếu có)"),
                      dropdownColor: primaryColor.withOpacity(0.9),
                      items: discountCodes.map((code) {
                        return DropdownMenuItem<String>(
                          value: code,
                          child: Text(code, style: TextStyle(fontFamily: 'Poppins', color: dropdownTextColor)),
                        );
                      }).toList(),
                      onChanged: (value) {
                        setState(() {
                          selectedDiscountCode = value;
                        });
                      },
                    ),
                    SizedBox(height: 30),
                    SizedBox(
                      width: double.infinity,
                      child: ElevatedButton(
                        onPressed: _isFormValid() ? () => _showConfirmationDialog(context) : null,
                        style: ElevatedButton.styleFrom(
                          backgroundColor: accentColor,
                          padding: EdgeInsets.symmetric(vertical: 16),
                          shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(12)),
                          elevation: 5,
                        ),
                        child: Text(
                          "Xác Nhận Đặt Lịch",
                          style: TextStyle(
                            fontFamily: 'Poppins',
                            fontSize: 18,
                            fontWeight: FontWeight.w600,
                            color: dropdownTextColor,
                          ),
                        ),
                      ),
                    ),
                  ],
                ],
              ),
            ),
    );
  }

  bool _isFormValid() {
    return selectedBranch != null &&
        selectedDateTime != null &&
        selectedEmployee != null &&
        selectedService != null;
  }

  void _showConfirmationDialog(BuildContext context) {
    final selectedBranchData = branches.firstWhere((b) => b['branchID'].toString() == selectedBranch);
    final selectedEmpData = employees.firstWhere((e) => e['empID'].toString() == selectedEmployee);
    final selectedServiceData = services.firstWhere((s) => s['servID'].toString() == selectedService);

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
            _buildConfirmationRow("Nhân viên", selectedEmpData['authUser']?['fullName'] ?? 'Unknown'),
            _buildConfirmationRow("Thời gian", DateFormat('dd/MM/yyyy - HH:mm').format(selectedDateTime!)),
            _buildConfirmationRow("Dịch vụ", selectedServiceData['servName']),
            if (selectedDiscountCode != null) _buildConfirmationRow("Mã giảm giá", selectedDiscountCode!),
          ],
        ),
        actions: [
          TextButton(
            onPressed: () => Navigator.pop(context),
            child: Text("Hủy", style: TextStyle(color: Colors.grey[400])),
          ),
          ElevatedButton(
            onPressed: () async {
              try {
                final bookingOrder = BookingCreateOrder(
                  appoint: [
                    BookingCreateBusinessAppoint(
                      servID: int.parse(selectedService!),
                      empID: int.parse(selectedEmployee!),
                      appStatus: "Pending",
                    ),
                  ],
                  order: BookingCreateOrderBusiness(
                    custID: 1, // Giả định, cần thay bằng ID từ AuthProvider
                    createAt: DateTime.now(),
                    orderDate: selectedDateTime!,
                  ),
                  promoID: selectedDiscountCode != null ? _mapDiscountCodeToPromoID(selectedDiscountCode!) : 0,
                );

                setState(() => isLoading = true);
                await _createBookingOrderUseCase.call(bookingOrder);
                setState(() => isLoading = false);

                Navigator.pop(context);
                ScaffoldMessenger.of(context).showSnackBar(
                  SnackBar(
                    content: Text("Đặt lịch thành công!", style: TextStyle(color: dropdownTextColor)),
                    backgroundColor: primaryColor,
                    behavior: SnackBarBehavior.floating,
                    shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(12)),
                  ),
                );
                Navigator.pop(context);
              } catch (e) {
                setState(() => isLoading = false);
                ScaffoldMessenger.of(context).showSnackBar(
                  SnackBar(
                    content: Text("Lỗi đặt lịch: $e", style: TextStyle(color: Colors.red)),
                    backgroundColor: primaryColor,
                    behavior: SnackBarBehavior.floating,
                    shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(12)),
                  ),
                );
              }
            },
            style: ElevatedButton.styleFrom(
              backgroundColor: accentColor,
              shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(10)),
            ),
            child: Text("Xác nhận", style: TextStyle(color: dropdownTextColor)),
          ),
        ],
      ),
    );
  }

  int _mapDiscountCodeToPromoID(String discountCode) {
    switch (discountCode) {
      case 'GIAM10':
        return 1;
      case 'GIAM20':
        return 2;
      case 'FREESHIP':
        return 3;
      default:
        return 0;
    }
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

  Widget _buildSectionTitle(String title) {
    return Padding(
      padding: const EdgeInsets.only(bottom: 8),
      child: Text(
        title,
        style: TextStyle(
          fontSize: 20,
          fontWeight: FontWeight.w600,
          color: textColor,
          fontFamily: 'Poppins',
        ),
      ),
    );
  }

  InputDecoration _inputDecoration(String label) {
    return InputDecoration(
      labelText: label,
      labelStyle: TextStyle(color: Colors.grey[400], fontFamily: 'Poppins'),
      border: OutlineInputBorder(
        borderRadius: BorderRadius.circular(12),
        borderSide: BorderSide(color: Colors.grey[600]!),
      ),
      focusedBorder: OutlineInputBorder(
        borderRadius: BorderRadius.circular(12),
        borderSide: BorderSide(color: accentColor, width: 2),
      ),
      filled: true,
      fillColor: primaryColor.withOpacity(0.2),
    );
  }
}