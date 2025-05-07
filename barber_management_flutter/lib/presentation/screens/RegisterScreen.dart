import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:intl/intl.dart';
import '../providers/auth_provider.dart';
import 'LoginScreen.dart';

class RegisterScreen extends StatefulWidget {
  @override
  _RegisterScreenState createState() => _RegisterScreenState();
}

class _RegisterScreenState extends State<RegisterScreen> {
  final TextEditingController userNameController = TextEditingController();
  final TextEditingController passwordController = TextEditingController();
  final TextEditingController emailController = TextEditingController();
  final TextEditingController phoneNumberController = TextEditingController();
  final TextEditingController fullNameController = TextEditingController();
  DateTime? selectedDate;
  String? selectedGender;
  final List<String> genders = ['Nam', 'Nữ', 'Khác'];

  static const primaryColor = Color(0xFF4E342E);
  static const backgroundColor = Color(0xFF212121);
  static const textColor = Color(0xFFEFEBE9);
  static const accentColor = Color(0xFF8D6E63);

  Future<void> _selectDate(BuildContext context) async {
    final DateTime? picked = await showDatePicker(
      context: context,
      initialDate: DateTime.now(),
      firstDate: DateTime(1900),
      lastDate: DateTime.now(),
      builder: (context, child) {
        return Theme(
          data: ThemeData.dark().copyWith(
            colorScheme: ColorScheme.dark(
              primary: accentColor,
              onPrimary: textColor,
              surface: primaryColor.withOpacity(0.9),
              onSurface: textColor,
            ),
          ),
          child: child!,
        );
      },
    );
    if (picked != null && picked != selectedDate) {
      setState(() {
        selectedDate = picked;
      });
    }
  }

  bool _validateInputs() {
    return userNameController.text.isNotEmpty &&
        passwordController.text.isNotEmpty &&
        emailController.text.isNotEmpty &&
        phoneNumberController.text.isNotEmpty &&
        fullNameController.text.isNotEmpty &&
        selectedDate != null &&
        selectedGender != null;
  }

  @override
  Widget build(BuildContext context) {
    final authProvider = Provider.of<AuthProvider>(context);

    return Scaffold(
      backgroundColor: backgroundColor,
      body: SafeArea(
        child: SingleChildScrollView(
          child: Padding(
            padding: const EdgeInsets.symmetric(horizontal: 24.0, vertical: 40.0),
            child: Column(
              mainAxisAlignment: MainAxisAlignment.center,
              crossAxisAlignment: CrossAxisAlignment.stretch,
              children: [
                Icon(Icons.person_add_rounded, size: 80, color: accentColor),
                SizedBox(height: 16),
                Text(
                  "Đăng ký",
                  textAlign: TextAlign.center,
                  style: TextStyle(
                    fontSize: 32,
                    fontWeight: FontWeight.bold,
                    color: textColor,
                    fontFamily: 'Poppins',
                  ),
                ),
                SizedBox(height: 40),
                TextField(
                  controller: fullNameController,
                  decoration: InputDecoration(
                    labelText: "Họ và tên",
                    labelStyle: TextStyle(color: Colors.grey[400]),
                    prefixIcon: Icon(Icons.person, color: accentColor),
                    border: OutlineInputBorder(borderRadius: BorderRadius.circular(12)),
                    focusedBorder: OutlineInputBorder(
                      borderRadius: BorderRadius.circular(12),
                      borderSide: BorderSide(color: accentColor, width: 2),
                    ),
                    filled: true,
                    fillColor: primaryColor.withOpacity(0.2),
                  ),
                  style: TextStyle(color: textColor),
                ),
                SizedBox(height: 20),
                TextField(
                  controller: userNameController,
                  decoration: InputDecoration(
                    labelText: "Tên đăng nhập",
                    labelStyle: TextStyle(color: Colors.grey[400]),
                    prefixIcon: Icon(Icons.account_circle, color: accentColor),
                    border: OutlineInputBorder(borderRadius: BorderRadius.circular(12)),
                    focusedBorder: OutlineInputBorder(
                      borderRadius: BorderRadius.circular(12),
                      borderSide: BorderSide(color: accentColor, width: 2),
                    ),
                    filled: true,
                    fillColor: primaryColor.withOpacity(0.2),
                  ),
                  style: TextStyle(color: textColor),
                ),
                SizedBox(height: 20),
                TextField(
                  controller: passwordController,
                  decoration: InputDecoration(
                    labelText: "Mật khẩu",
                    labelStyle: TextStyle(color: Colors.grey[400]),
                    prefixIcon: Icon(Icons.lock_outline, color: accentColor),
                    border: OutlineInputBorder(borderRadius: BorderRadius.circular(12)),
                    focusedBorder: OutlineInputBorder(
                      borderRadius: BorderRadius.circular(12),
                      borderSide: BorderSide(color: accentColor, width: 2),
                    ),
                    filled: true,
                    fillColor: primaryColor.withOpacity(0.2),
                  ),
                  obscureText: true,
                  style: TextStyle(color: textColor),
                ),
                SizedBox(height: 20),
                TextField(
                  controller: emailController,
                  decoration: InputDecoration(
                    labelText: "Email",
                    labelStyle: TextStyle(color: Colors.grey[400]),
                    prefixIcon: Icon(Icons.email_outlined, color: accentColor),
                    border: OutlineInputBorder(borderRadius: BorderRadius.circular(12)),
                    focusedBorder: OutlineInputBorder(
                      borderRadius: BorderRadius.circular(12),
                      borderSide: BorderSide(color: accentColor, width: 2),
                    ),
                    filled: true,
                    fillColor: primaryColor.withOpacity(0.2),
                  ),
                  keyboardType: TextInputType.emailAddress,
                  style: TextStyle(color: textColor),
                ),
                SizedBox(height: 20),
                TextField(
                  controller: phoneNumberController,
                  decoration: InputDecoration(
                    labelText: "Số điện thoại",
                    labelStyle: TextStyle(color: Colors.grey[400]),
                    prefixIcon: Icon(Icons.phone, color: accentColor),
                    border: OutlineInputBorder(borderRadius: BorderRadius.circular(12)),
                    focusedBorder: OutlineInputBorder(
                      borderRadius: BorderRadius.circular(12),
                      borderSide: BorderSide(color: accentColor, width: 2),
                    ),
                    filled: true,
                    fillColor: primaryColor.withOpacity(0.2),
                  ),
                  keyboardType: TextInputType.phone,
                  style: TextStyle(color: textColor),
                ),
                SizedBox(height: 20),
                GestureDetector(
                  onTap: () => _selectDate(context),
                  child: Container(
                    padding: EdgeInsets.symmetric(vertical: 16, horizontal: 12),
                    decoration: BoxDecoration(
                      border: Border.all(color: Colors.grey[400]!),
                      borderRadius: BorderRadius.circular(12),
                      color: primaryColor.withOpacity(0.2),
                    ),
                    child: Row(
                      children: [
                        Icon(Icons.calendar_today, color: accentColor),
                        SizedBox(width: 12),
                        Text(
                          selectedDate == null
                              ? "Ngày sinh"
                              : DateFormat('dd/MM/yyyy').format(selectedDate!),
                          style: TextStyle(
                            color: selectedDate == null ? Colors.grey[400] : textColor,
                            fontFamily: 'Poppins',
                          ),
                        ),
                      ],
                    ),
                  ),
                ),
                SizedBox(height: 20),
                DropdownButtonFormField<String>(
                  value: selectedGender,
                  decoration: InputDecoration(
                    labelText: "Giới tính",
                    labelStyle: TextStyle(color: Colors.grey[400]),
                    prefixIcon: Icon(Icons.person_outline, color: accentColor),
                    border: OutlineInputBorder(borderRadius: BorderRadius.circular(12)),
                    focusedBorder: OutlineInputBorder(
                      borderRadius: BorderRadius.circular(12),
                      borderSide: BorderSide(color: accentColor, width: 2),
                    ),
                    filled: true,
                    fillColor: primaryColor.withOpacity(0.2),
                  ),
                  dropdownColor: primaryColor.withOpacity(0.9),
                  style: TextStyle(color: textColor),
                  items: genders.map((gender) {
                    return DropdownMenuItem(
                      value: gender,
                      child: Text(gender, style: TextStyle(color: textColor)),
                    );
                  }).toList(),
                  onChanged: (value) {
                    setState(() {
                      selectedGender = value;
                    });
                  },
                ),
                SizedBox(height: 30),
                authProvider.isLoading
                    ? Center(child: CircularProgressIndicator(valueColor: AlwaysStoppedAnimation<Color>(accentColor)))
                    : ElevatedButton(
                        onPressed: _validateInputs()
                            ? () async {
                                final dateOfBirth = DateFormat("yyyy-MM-dd'T'HH:mm:ss.SSS'Z'").format(selectedDate!);
                                await authProvider.register(
                                  roleID: 1, // Hardcoded as per API example
                                  userName: userNameController.text,
                                  password: passwordController.text,
                                  email: emailController.text,
                                  phoneNumber: phoneNumberController.text,
                                  fullName: fullNameController.text,
                                  dateOfBirth: dateOfBirth,
                                  gender: selectedGender!,
                                );
                                if (authProvider.user != null) {
                                  ScaffoldMessenger.of(context).showSnackBar(
                                    SnackBar(
                                      content: Text("Đăng ký thành công. Vui lòng kiểm tra email để xác nhận."),
                                      backgroundColor: Colors.green,
                                    ),
                                  );
                                  Navigator.pushReplacement(
                                    context,
                                    MaterialPageRoute(builder: (context) => LoginScreen()),
                                  );
                                } else if (authProvider.errorMessage != null) {
                                  ScaffoldMessenger.of(context).showSnackBar(
                                    SnackBar(
                                      content: Text(authProvider.errorMessage!),
                                      backgroundColor: Colors.red,
                                    ),
                                  );
                                }
                              }
                            : null,
                        style: ElevatedButton.styleFrom(
                          padding: EdgeInsets.symmetric(vertical: 16),
                          shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(12)),
                          backgroundColor: _validateInputs() ? accentColor : Colors.grey,
                        ),
                        child: Text(
                          "Đăng ký",
                          style: TextStyle(fontSize: 18, color: Colors.white, fontFamily: 'Poppins'),
                        ),
                      ),
                SizedBox(height: 20),
                TextButton(
                  onPressed: () {
                    Navigator.push(
                      context,
                      MaterialPageRoute(builder: (context) => LoginScreen()),
                    );
                  },
                  child: Text(
                    "Đã có tài khoản? Đăng nhập",
                    style: TextStyle(color: accentColor, fontFamily: 'Poppins'),
                  ),
                ),
              ],
            ),
          ),
        ),
      ),
    );
  }
}