import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:intl/intl.dart';
import 'package:flutter_animate/flutter_animate.dart';
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
  final _formKey = GlobalKey<FormState>();
  DateTime? selectedDate;
  String? selectedGender;
  bool _isPasswordVisible = false;
  final List<String> genders = ['Nam', 'Nữ', 'Khác'];

  static const primaryColor = Color(0xFF4E342E);
  static const backgroundColor = Color(0xFF212121);
  static const textColor = Color(0xFFEFEBE9);
  static const accentColor = Color(0xFF8D6E63);

  @override
  void dispose() {
    userNameController.dispose();
    passwordController.dispose();
    emailController.dispose();
    phoneNumberController.dispose();
    fullNameController.dispose();
    super.dispose();
  }

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
              surface: primaryColor,
              onSurface: textColor,
            ),
            dialogBackgroundColor: primaryColor.withOpacity(0.9),
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

  String? _validatePassword(String? value) {
    if (value == null || value.isEmpty) return 'Vui lòng nhập mật khẩu';
    if (value.length < 8) return 'Mật khẩu phải có ít nhất 8 ký tự';
    return null;
  }

  @override
  Widget build(BuildContext context) {
    final authProvider = Provider.of<AuthProvider>(context);

    return Scaffold(
      body: Container(
        decoration: BoxDecoration(
          gradient: LinearGradient(
            begin: Alignment.topLeft,
            end: Alignment.bottomRight,
            colors: [backgroundColor, backgroundColor.withOpacity(0.9)],
          ),
        ),
        child: SafeArea(
          child: Center(
            child: SingleChildScrollView(
              padding: EdgeInsets.symmetric(horizontal: 20.0),
              child: Form(
                key: _formKey,
                child: Column(
                  mainAxisSize: MainAxisSize.min,
                  children: [
                    // Title
                    Text(
                      'Barber Register',
                      style: TextStyle(
                        fontFamily: 'Montserrat',
                        fontSize: 32,
                        fontWeight: FontWeight.bold,
                        foreground: Paint()
                          ..shader = LinearGradient(
                            colors: [accentColor, textColor],
                          ).createShader(Rect.fromLTWH(0, 0, 200, 40)),
                      ),
                    )
                        .animate()
                        .fadeIn(duration: 600.ms)
                        .slideY(begin: -0.2, end: 0, curve: Curves.easeOut),
                    SizedBox(height: 20),
                    // Glassmorphic form container
                    Container(
                      padding: EdgeInsets.all(24),
                      decoration: BoxDecoration(
                        color: primaryColor.withOpacity(0.2),
                        borderRadius: BorderRadius.circular(16),
                        border: Border.all(color: Colors.white.withOpacity(0.1)),
                        boxShadow: [
                          BoxShadow(
                            color: Colors.black.withOpacity(0.3),
                            blurRadius: 10,
                            offset: Offset(0, 4),
                          ),
                        ],
                      ),
                      child: Column(
                        children: [
                          // Full Name
                          TextFormField(
                            controller: fullNameController,
                            decoration: InputDecoration(
                              labelText: 'Họ và tên',
                              labelStyle: TextStyle(color: Colors.grey[400], fontFamily: 'Montserrat'),
                              prefixIcon: Icon(Icons.person, color: accentColor),
                              filled: true,
                              fillColor: Colors.white.withOpacity(0.05),
                              border: OutlineInputBorder(
                                borderRadius: BorderRadius.circular(12),
                                borderSide: BorderSide.none,
                              ),
                              focusedBorder: OutlineInputBorder(
                                borderRadius: BorderRadius.circular(12),
                                borderSide: BorderSide(color: accentColor, width: 2),
                              ),
                              errorBorder: OutlineInputBorder(
                                borderRadius: BorderRadius.circular(12),
                                borderSide: BorderSide(color: Colors.red),
                              ),
                            ),
                            style: TextStyle(color: textColor, fontFamily: 'Montserrat'),
                            validator: (value) =>
                                value == null || value.isEmpty ? 'Vui lòng nhập họ và tên' : null,
                          )
                              .animate()
                              .fadeIn(delay: 200.ms, duration: 400.ms)
                              .slideX(begin: -0.1, end: 0),
                          SizedBox(height: 16),
                          // Username
                          TextFormField(
                            controller: userNameController,
                            decoration: InputDecoration(
                              labelText: 'Tên đăng nhập',
                              labelStyle: TextStyle(color: Colors.grey[400], fontFamily: 'Montserrat'),
                              prefixIcon: Icon(Icons.account_circle, color: accentColor),
                              filled: true,
                              fillColor: Colors.white.withOpacity(0.05),
                              border: OutlineInputBorder(
                                borderRadius: BorderRadius.circular(12),
                                borderSide: BorderSide.none,
                              ),
                              focusedBorder: OutlineInputBorder(
                                borderRadius: BorderRadius.circular(12),
                                borderSide: BorderSide(color: accentColor, width: 2),
                              ),
                              errorBorder: OutlineInputBorder(
                                borderRadius: BorderRadius.circular(12),
                                borderSide: BorderSide(color: Colors.red),
                              ),
                            ),
                            style: TextStyle(color: textColor, fontFamily: 'Montserrat'),
                            validator: (value) =>
                                value == null || value.isEmpty ? 'Vui lòng nhập tên đăng nhập' : null,
                          )
                              .animate()
                              .fadeIn(delay: 300.ms, duration: 400.ms)
                              .slideX(begin: -0.1, end: 0),
                          SizedBox(height: 16),
                          // Password
                          TextFormField(
                            controller: passwordController,
                            decoration: InputDecoration(
                              labelText: 'Mật khẩu',
                              labelStyle: TextStyle(color: Colors.grey[400], fontFamily: 'Montserrat'),
                              prefixIcon: Icon(Icons.lock, color: accentColor),
                              suffixIcon: IconButton(
                                icon: Icon(
                                  _isPasswordVisible ? Icons.visibility : Icons.visibility_off,
                                  color: Colors.grey[400],
                                ),
                                onPressed: () => setState(() => _isPasswordVisible = !_isPasswordVisible),
                              ),
                              filled: true,
                              fillColor: Colors.white.withOpacity(0.05),
                              border: OutlineInputBorder(
                                borderRadius: BorderRadius.circular(12),
                                borderSide: BorderSide.none,
                              ),
                              focusedBorder: OutlineInputBorder(
                                borderRadius: BorderRadius.circular(12),
                                borderSide: BorderSide(color: accentColor, width: 2),
                              ),
                              errorBorder: OutlineInputBorder(
                                borderRadius: BorderRadius.circular(12),
                                borderSide: BorderSide(color: Colors.red),
                              ),
                            ),
                            obscureText: !_isPasswordVisible,
                            style: TextStyle(color: textColor, fontFamily: 'Montserrat'),
                            validator: _validatePassword,
                          )
                              .animate()
                              .fadeIn(delay: 400.ms, duration: 400.ms)
                              .slideX(begin: -0.1, end: 0),
                          SizedBox(height: 16),
                          // Email
                          TextFormField(
                            controller: emailController,
                            decoration: InputDecoration(
                              labelText: 'Email',
                              labelStyle: TextStyle(color: Colors.grey[400], fontFamily: 'Montserrat'),
                              prefixIcon: Icon(Icons.email, color: accentColor),
                              filled: true,
                              fillColor: Colors.white.withOpacity(0.05),
                              border: OutlineInputBorder(
                                borderRadius: BorderRadius.circular(12),
                                borderSide: BorderSide.none,
                              ),
                              focusedBorder: OutlineInputBorder(
                                borderRadius: BorderRadius.circular(12),
                                borderSide: BorderSide(color: accentColor, width: 2),
                              ),
                              errorBorder: OutlineInputBorder(
                                borderRadius: BorderRadius.circular(12),
                                borderSide: BorderSide(color: Colors.red),
                              ),
                            ),
                            keyboardType: TextInputType.emailAddress,
                            style: TextStyle(color: textColor, fontFamily: 'Montserrat'),
                            validator: (value) {
                              if (value == null || value.isEmpty) return 'Vui lòng nhập email';
                              if (!RegExp(r'^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$').hasMatch(value)) {
                                return 'Email không hợp lệ';
                              }
                              return null;
                            },
                          )
                              .animate()
                              .fadeIn(delay: 500.ms, duration: 400.ms)
                              .slideX(begin: -0.1, end: 0),
                          SizedBox(height: 16),
                          // Phone Number
                          TextFormField(
                            controller: phoneNumberController,
                            decoration: InputDecoration(
                              labelText: 'Số điện thoại',
                              labelStyle: TextStyle(color: Colors.grey[400], fontFamily: 'Montserrat'),
                              prefixIcon: Icon(Icons.phone, color: accentColor),
                              filled: true,
                              fillColor: Colors.white.withOpacity(0.05),
                              border: OutlineInputBorder(
                                borderRadius: BorderRadius.circular(12),
                                borderSide: BorderSide.none,
                              ),
                              focusedBorder: OutlineInputBorder(
                                borderRadius: BorderRadius.circular(12),
                                borderSide: BorderSide(color: accentColor, width: 2),
                              ),
                              errorBorder: OutlineInputBorder(
                                borderRadius: BorderRadius.circular(12),
                                borderSide: BorderSide(color: Colors.red),
                              ),
                            ),
                            keyboardType: TextInputType.phone,
                            style: TextStyle(color: textColor, fontFamily: 'Montserrat'),
                            validator: (value) =>
                                value == null || value.isEmpty ? 'Vui lòng nhập số điện thoại' : null,
                          )
                              .animate()
                              .fadeIn(delay: 600.ms, duration: 400.ms)
                              .slideX(begin: -0.1, end: 0),
                          SizedBox(height: 16),
                          // Date of Birth
                          GestureDetector(
                            onTap: () => _selectDate(context),
                            child: Container(
                              padding: EdgeInsets.symmetric(vertical: 16, horizontal: 12),
                              decoration: BoxDecoration(
                                color: Colors.white.withOpacity(0.05),
                                borderRadius: BorderRadius.circular(12),
                              ),
                              child: Row(
                                children: [
                                  Icon(Icons.calendar_today, color: accentColor),
                                  SizedBox(width: 12),
                                  Text(
                                    selectedDate == null
                                        ? 'Ngày sinh'
                                        : DateFormat('dd/MM/yyyy').format(selectedDate!),
                                    style: TextStyle(
                                      color: selectedDate == null ? Colors.grey[400] : textColor,
                                      fontFamily: 'Montserrat',
                                    ),
                                  ),
                                ],
                              ),
                            ),
                          )
                              .animate()
                              .fadeIn(delay: 700.ms, duration: 400.ms)
                              .slideX(begin: -0.1, end: 0),
                          SizedBox(height: 16),
                          // Gender
                          DropdownButtonFormField<String>(
                            value: selectedGender,
                            decoration: InputDecoration(
                              labelText: 'Giới tính',
                              labelStyle: TextStyle(color: Colors.grey[400], fontFamily: 'Montserrat'),
                              prefixIcon: Icon(Icons.person_outline, color: accentColor),
                              filled: true,
                              fillColor: Colors.white.withOpacity(0.05),
                              border: OutlineInputBorder(
                                borderRadius: BorderRadius.circular(12),
                                borderSide: BorderSide.none,
                              ),
                              focusedBorder: OutlineInputBorder(
                                borderRadius: BorderRadius.circular(12),
                                borderSide: BorderSide(color: accentColor, width: 2),
                              ),
                              errorBorder: OutlineInputBorder(
                                borderRadius: BorderRadius.circular(12),
                                borderSide: BorderSide(color: Colors.red),
                              ),
                            ),
                            dropdownColor: primaryColor,
                            style: TextStyle(color: textColor, fontFamily: 'Montserrat'),
                            items: genders.map((gender) {
                              return DropdownMenuItem(
                                value: gender,
                                child: Text(gender, style: TextStyle(color: textColor)),
                              );
                            }).toList(),
                            onChanged: (value) => setState(() => selectedGender = value),
                            validator: (value) => value == null ? 'Vui lòng chọn giới tính' : null,
                          )
                              .animate()
                              .fadeIn(delay: 800.ms, duration: 400.ms)
                              .slideX(begin: -0.1, end: 0),
                          SizedBox(height: 24),
                          // Register button
                          authProvider.isLoading
                              ? Center(
                                  child: CircularProgressIndicator(
                                    valueColor: AlwaysStoppedAnimation<Color>(accentColor),
                                  ),
                                )
                              : InkWell(
                                  onTap: () async {
                                    if (_formKey.currentState!.validate() && selectedDate != null) {
                                      final dateOfBirth =
                                          DateFormat("yyyy-MM-dd'T'HH:mm:ss.SSS'Z'").format(selectedDate!);
                                      await authProvider.register(
                                        roleID: 1,
                                        userName: userNameController.text,
                                        password: passwordController.text,
                                        email: emailController.text,
                                        phoneNumber: phoneNumberController.text,
                                        fullName: fullNameController.text,
                                        dateOfBirth: dateOfBirth,
                                        gender: selectedGender!,
                                      );
                                      // Clear user state to prevent auto-login
                                      await authProvider.logout();
                                      ScaffoldMessenger.of(context).showSnackBar(
                                        SnackBar(
                                          content: Text('Đăng ký thành công. Vui lòng kiểm tra email'),
                                          backgroundColor: Colors.green,
                                          behavior: SnackBarBehavior.floating,
                                        ),
                                      );
                                      // Navigate to LoginScreen and clear stack
                                      Navigator.pushAndRemoveUntil(
                                        context,
                                        MaterialPageRoute(builder: (context) => LoginScreen()),
                                        (route) => false,
                                      );
                                    } else if (selectedDate == null) {
                                      ScaffoldMessenger.of(context).showSnackBar(
                                        SnackBar(
                                          content: Text('Vui lòng chọn ngày sinh'),
                                          backgroundColor: Colors.red,
                                          behavior: SnackBarBehavior.floating,
                                        ),
                                      );
                                    }
                                  },
                                  borderRadius: BorderRadius.circular(16),
                                  child: Container(
                                    padding: EdgeInsets.symmetric(vertical: 12),
                                    decoration: BoxDecoration(
                                      gradient: LinearGradient(
                                        colors: [accentColor, Color(0xFFB89778)],
                                        begin: Alignment.topLeft,
                                        end: Alignment.bottomRight,
                                      ),
                                      borderRadius: BorderRadius.only(
                                        topLeft: Radius.circular(16),
                                        topRight: Radius.circular(8),
                                        bottomLeft: Radius.circular(8),
                                        bottomRight: Radius.circular(16),
                                      ),
                                      boxShadow: [
                                        BoxShadow(
                                          color: accentColor.withOpacity(0.6),
                                          blurRadius: 12,
                                          offset: Offset(0, 4),
                                        ),
                                        BoxShadow(
                                          color: Colors.black.withOpacity(0.2),
                                          blurRadius: 8,
                                          offset: Offset(0, -2),
                                        ),
                                      ],
                                    ),
                                    child: Row(
                                      mainAxisAlignment: MainAxisAlignment.center,
                                      children: [
                                        Icon(Icons.person_add, color: Colors.white, size: 18),
                                        SizedBox(width: 8),
                                        Text(
                                          'Đăng ký',
                                          style: TextStyle(
                                            fontSize: 18,
                                            color: Colors.white,
                                            fontFamily: 'Montserrat',
                                            fontWeight: FontWeight.w700,
                                            letterSpacing: 1.2,
                                          ),
                                        ),
                                      ],
                                    ),
                                  ),
                                )
                                  .animate()
                                  .fadeIn(delay: 900.ms, duration: 400.ms)
                                  .scale(curve: Curves.easeOut)
                                  .then()
                                  .shimmer(duration: 600.ms, color: Colors.white.withOpacity(0.2)),
                          SizedBox(height: 16),
                          // Login link
                          TextButton(
                            onPressed: () {
                              Navigator.push(
                                context,
                                MaterialPageRoute(builder: (context) => LoginScreen()),
                              );
                            },
                            child: Text(
                              'Đã có tài khoản? Đăng nhập',
                              style: TextStyle(
                                color: accentColor,
                                fontFamily: 'Montserrat',
                                fontSize: 14,
                                // decoration: TextDecoration.underline,
                              ),
                            ),
                          ).animate().fadeIn(delay: 1000.ms, duration: 400.ms),
                        ],
                      ),
                    ),
                  ],
                ),
              ),
            ),
          ),
        ),
      ),
    );
  }
}