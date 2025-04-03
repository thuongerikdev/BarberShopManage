import 'package:flutter/material.dart';
import 'package:intl/intl.dart';

class BookingScreen extends StatefulWidget {
  @override
  _BookingScreenState createState() => _BookingScreenState();
}

class _BookingScreenState extends State<BookingScreen> {
  final List<Map<String, dynamic>> services = [
    {'name': 'Cắt tóc', 'employees': ['Nguyễn Văn A', 'Trần Thị B'], 'price': 100000},
    {'name': 'Nhuộm tóc', 'employees': ['Lê Văn C'], 'price': 250000},
    {'name': 'Massage', 'employees': ['Nguyễn Văn A', 'Lê Văn C'], 'price': 150000},
  ];

  final List<String> branches = ['Cơ sở 1 - Hà Nội', 'Cơ sở 2 - TP.HCM', 'Cơ sở 3 - Đà Nẵng'];
  final List<String> discountCodes = ['GIAM10', 'GIAM20', 'FREESHIP'];

  String? selectedService;
  String? selectedEmployee;
  DateTime? selectedDateTime;
  String? selectedBranch;
  String? selectedDiscountCode;
  List<String> availableEmployees = [];

  static const primaryColor = Color(0xFF4E342E); // Nâu đậm
  static const backgroundColor = Color(0xFF212121); // Đen xám
  static const textColor = Color(0xFFEFEBE9); // Xám nhạt (vẫn dùng cho text chính)
  static const accentColor = Color(0xFF8D6E63); // Nâu nhạt
  static const dropdownTextColor = Color(0xFFD7CCC8); // Màu nâu xám nhạt cho dropdown và dialog

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
            color: dropdownTextColor, // Đổi từ trắng sang nâu xám nhạt
          ),
        ),
        leading: IconButton(
          icon: Icon(Icons.arrow_back, color: dropdownTextColor),
          onPressed: () => Navigator.pop(context),
        ),
      ),
      body: SingleChildScrollView(
        padding: EdgeInsets.all(20),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            _buildSectionTitle("Chọn Dịch Vụ"),
            DropdownButtonFormField<String>(
              value: selectedService,
              decoration: _inputDecoration("Dịch vụ"),
              dropdownColor: primaryColor.withOpacity(0.9), // Nền dropdown gần giống primaryColor
              items: services.map((service) {
                return DropdownMenuItem<String>(
                  value: service['name'],
                  child: Text(
                    "${service['name']} - ${NumberFormat.currency(locale: 'vi_VN', symbol: '₫').format(service['price'])}",
                    style: TextStyle(fontFamily: 'Poppins', color: dropdownTextColor),
                  ),
                );
              }).toList(),
              onChanged: (value) {
                setState(() {
                  selectedService = value;
                  selectedEmployee = null;
                  availableEmployees = services
                      .firstWhere((service) => service['name'] == value)['employees'];
                });
              },
            ),
            SizedBox(height: 20),
            if (selectedService != null) ...[
              _buildSectionTitle("Chọn Nhân Viên"),
              DropdownButtonFormField<String>(
                value: selectedEmployee,
                decoration: _inputDecoration("Nhân viên"),
                dropdownColor: primaryColor.withOpacity(0.9),
                items: availableEmployees.map((employee) {
                  return DropdownMenuItem<String>(
                    value: employee,
                    child: Text(employee, style: TextStyle(fontFamily: 'Poppins', color: dropdownTextColor)),
                  );
                }).toList(),
                onChanged: (value) {
                  setState(() {
                    selectedEmployee = value;
                  });
                },
              ),
              SizedBox(height: 20),
            ],
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
                          primary: accentColor, // Đổi màu nút chọn thành accentColor
                          onPrimary: dropdownTextColor, // Văn bản trên nút
                          surface: primaryColor.withOpacity(0.9), // Nền của picker
                          onSurface: dropdownTextColor, // Văn bản ngày
                        ),
                        dialogBackgroundColor: backgroundColor,
                        textTheme: TextTheme(
                          bodyMedium: TextStyle(color: dropdownTextColor), // Thay bodyText2 bằng bodyMedium
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
                            onPrimary: dropdownTextColor,
                            surface: primaryColor.withOpacity(0.9),
                            onSurface: dropdownTextColor,
                          ),
                          dialogBackgroundColor: backgroundColor,
                          textTheme: TextTheme(
                            bodyMedium: TextStyle(color: dropdownTextColor), // Thay bodyText2 bằng bodyMedium
                          ),
                        ),
                        child: child!,
                      );
                    },
                  );
                  if (time != null) {
                    setState(() {
                      selectedDateTime = DateTime(
                        date.year,
                        date.month,
                        date.day,
                        time.hour,
                        time.minute,
                      );
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
            _buildSectionTitle("Chọn Cơ Sở"),
            DropdownButtonFormField<String>(
              value: selectedBranch,
              decoration: _inputDecoration("Cơ sở"),
              dropdownColor: primaryColor.withOpacity(0.9),
              items: branches.map((branch) {
                return DropdownMenuItem<String>(
                  value: branch,
                  child: Text(branch, style: TextStyle(fontFamily: 'Poppins', color: dropdownTextColor)),
                );
              }).toList(),
              onChanged: (value) {
                setState(() {
                  selectedBranch = value;
                });
              },
            ),
            SizedBox(height: 20),
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
                onPressed: _isFormValid()
                    ? () {
                        _showConfirmationDialog(context);
                      }
                    : null,
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
                    color: dropdownTextColor, // Đổi từ trắng sang nâu xám nhạt
                  ),
                ),
              ),
            ),
          ],
        ),
      ),
    );
  }

  bool _isFormValid() {
    return selectedService != null &&
        selectedEmployee != null &&
        selectedDateTime != null &&
        selectedBranch != null;
  }

  void _showConfirmationDialog(BuildContext context) {
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
            _buildConfirmationRow("Dịch vụ", selectedService!),
            _buildConfirmationRow("Nhân viên", selectedEmployee!),
            _buildConfirmationRow(
                "Thời gian", DateFormat('dd/MM/yyyy - HH:mm').format(selectedDateTime!)),
            _buildConfirmationRow("Cơ sở", selectedBranch!),
            if (selectedDiscountCode != null)
              _buildConfirmationRow("Mã giảm giá", selectedDiscountCode!),
          ],
        ),
        actions: [
          TextButton(
            onPressed: () => Navigator.pop(context),
            child: Text("Hủy", style: TextStyle(color: Colors.grey[400])),
          ),
          ElevatedButton(
            onPressed: () {
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
      padding: const EdgeInsets.only(bottom: 8), // Sửa lỗi typo: bottom thay vì custom
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