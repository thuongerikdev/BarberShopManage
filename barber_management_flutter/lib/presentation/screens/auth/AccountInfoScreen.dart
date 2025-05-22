import 'package:barbermanagemobile/data/models/auth/user_model.dart';
import 'package:flutter/material.dart';
import 'package:get_it/get_it.dart';
import 'package:intl/intl.dart';
import 'package:provider/provider.dart';
import 'package:barbermanagemobile/domain/usecases/get_user_by_id_use_case.dart';
import 'package:barbermanagemobile/presentation/providers/auth_provider.dart';
import 'package:image_picker/image_picker.dart';
import 'dart:io';
import 'package:dartz/dartz.dart' as dz;

class AccountInfoScreen extends StatefulWidget {
  static const primaryColor = Color(0xFF4E342E);
  static const backgroundColor = Color(0xFF212121);
  static const textColor = Color(0xFFEFEBE9);
  static const accentColor = Color(0xFF8D6E63);
  static const dropdownTextColor = Color(0xFFD7CCC8);

  const AccountInfoScreen({super.key});

  @override
  State<AccountInfoScreen> createState() => _AccountInfoScreenState();
}

class _AccountInfoScreenState extends State<AccountInfoScreen> {
  final ImagePicker _picker = ImagePicker();
  final _formKey = GlobalKey<FormState>();
  bool _isEditing = false;
  UserModel? _currentUserData;

  final _fullNameController = TextEditingController();
  final _userNameController = TextEditingController();
  final _emailController = TextEditingController();
  final _phoneController = TextEditingController();
  final _dateOfBirthController = TextEditingController();
  final _genderController = TextEditingController();

  @override
  void initState() {
    super.initState();
    _fullNameController.text = '';
    _userNameController.text = '';
    _emailController.text = '';
    _phoneController.text = '';
    _dateOfBirthController.text = '';
    _genderController.text = '';
  }

  Future<void> _pickAndUploadAvatar(BuildContext context) async {
    try {
      final XFile? pickedFile = await _picker.pickImage(source: ImageSource.gallery);
      if (pickedFile == null) return;

      final File image = File(pickedFile.path);
      final authProvider = Provider.of<AuthProvider>(context, listen: false);

      await authProvider.updateAvatar(image);
      if (authProvider.errorMessage != null) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text("Lỗi khi cập nhật ảnh: ${authProvider.errorMessage}"),
            backgroundColor: AccountInfoScreen.primaryColor,
          ),
        );
      } else {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: const Text("Cập nhật ảnh đại diện thành công"),
            backgroundColor: AccountInfoScreen.accentColor,
          ),
        );
      }
    } catch (e) {
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(
          content: Text("Lỗi: $e"),
          backgroundColor: AccountInfoScreen.primaryColor,
        ),
      );
    }
  }

  String _formatPhoneNumber(String? phone) {
    if (phone == null || phone.isEmpty) return '';
    String cleanedPhone = phone.replaceAll(RegExp(r'[^0-9+]'), '');
    if (cleanedPhone.startsWith('0')) {
      cleanedPhone = '+84${cleanedPhone.substring(1)}';
    }
    return cleanedPhone;
  }

  String _formatEmail(String? email) {
    if (email == null || email.isEmpty) return '';
    return email.trim();
  }

  String _formatDisplayDate(String? date) {
    if (date == null) return '';
    try {
      final parsedDate = DateTime.parse(date);
      return DateFormat('dd-MM-yyyy').format(parsedDate);
    } catch (e) {
      return '';
    }
  }

  void _toggleEditMode() {
    setState(() {
      _isEditing = !_isEditing;
    });
    if (_isEditing && _currentUserData != null) {
      _fullNameController.text = _currentUserData!.name ?? '';
      _userNameController.text = _currentUserData!.userName ?? '';
      _emailController.text = _formatEmail(_currentUserData!.email);
      _phoneController.text = _formatPhoneNumber(_currentUserData!.phoneNumber);
      _dateOfBirthController.text = _formatDisplayDate(_currentUserData!.dateOfBirth);
      _genderController.text = _currentUserData!.gender ?? '';
    }
  }

  Future<void> _saveChanges(BuildContext context) async {
    if (_formKey.currentState!.validate()) {
      final authProvider = Provider.of<AuthProvider>(context, listen: false);
      final userIdInt = int.parse(authProvider.user!.userId);

      await authProvider.updateUserInfo(
        userID: userIdInt,
        fullName: _fullNameController.text,
        userName: _userNameController.text.isNotEmpty ? _userNameController.text : null,
        userEmail: _emailController.text.isNotEmpty ? _emailController.text : null,
        userPhone: _phoneController.text.isNotEmpty ? _phoneController.text : null,
        dateOfBirth: _dateOfBirthController.text.isNotEmpty
            ? DateFormat('yyyy-MM-dd').format(DateFormat('dd-MM-yyyy').parse(_dateOfBirthController.text))
            : null,
        gender: _genderController.text.isNotEmpty ? _genderController.text : null,
      );

      if (authProvider.errorMessage == null) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: const Text("Cập nhật thông tin thành công"),
            backgroundColor: AccountInfoScreen.accentColor,
          ),
        );
        final updatedUser = await GetIt.instance<GetUserByIDUseCase>().call(userIdInt);
        updatedUser.fold(
          (error) => ScaffoldMessenger.of(context).showSnackBar(
            SnackBar(
              content: Text("Lỗi khi làm mới dữ liệu: $error"),
              backgroundColor: AccountInfoScreen.primaryColor,
            ),
          ),
          (userData) {
            authProvider.updateUser(userData);
            setState(() {
              _currentUserData = userData;
              _isEditing = false;
            });
          },
        );
      } else {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text("Lỗi: ${authProvider.errorMessage}"),
            backgroundColor: AccountInfoScreen.primaryColor,
          ),
        );
      }
    }
  }

  @override
  void dispose() {
    _fullNameController.dispose();
    _userNameController.dispose();
    _emailController.dispose();
    _phoneController.dispose();
    _dateOfBirthController.dispose();
    _genderController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    final authProvider = Provider.of<AuthProvider>(context);
    final user = authProvider.user;

    if (user?.userId == null || int.tryParse(user!.userId) == null) {
      return Scaffold(
        backgroundColor: AccountInfoScreen.backgroundColor,
        appBar: AppBar(
          elevation: 0,
          backgroundColor: AccountInfoScreen.primaryColor,
          title: const Text(
            "THÔNG TIN TÀI KHOẢN",
            style: TextStyle(
              fontFamily: 'Poppins',
              fontWeight: FontWeight.w700,
              fontSize: 24,
              color: AccountInfoScreen.dropdownTextColor,
            ),
          ),
          leading: IconButton(
            icon: const Icon(Icons.arrow_back, color: AccountInfoScreen.dropdownTextColor),
            onPressed: () => Navigator.pop(context),
          ),
        ),
        body: Center(
          child: Text(
            "Không tìm thấy thông tin người dùng",
            style: TextStyle(
              fontSize: 18,
              color: AccountInfoScreen.textColor,
              fontFamily: 'Poppins',
            ),
          ),
        ),
      );
    }

    final userIdInt = int.parse(user.userId);
    final getUserByIDUseCase = GetIt.instance<GetUserByIDUseCase>();

    return Scaffold(
      backgroundColor: AccountInfoScreen.backgroundColor,
      appBar: AppBar(
        elevation: 0,
        backgroundColor: AccountInfoScreen.primaryColor,
        title: const Text(
          "THÔNG TIN TÀI KHOẢN",
          style: TextStyle(
            fontFamily: 'Poppins',
            fontWeight: FontWeight.w700,
            fontSize: 24,
            color: AccountInfoScreen.dropdownTextColor,
          ),
        ),
        leading: IconButton(
          icon: const Icon(Icons.arrow_back, color: AccountInfoScreen.dropdownTextColor),
          onPressed: () => Navigator.pop(context),
        ),
        actions: [
          TextButton(
            onPressed: authProvider.isLoading
                ? null
                : () => _isEditing ? _saveChanges(context) : _toggleEditMode(),
            child: Text(
              _isEditing ? "Lưu" : "Chỉnh sửa",
              style: TextStyle(
                fontSize: 16,
                color: AccountInfoScreen.accentColor,
                fontFamily: 'Poppins',
                fontWeight: FontWeight.w600,
              ),
            ),
          ),
        ],
      ),
      body: Stack(
        children: [
          FutureBuilder<dz.Either<String, UserModel>>(
            future: getUserByIDUseCase.call(userIdInt),
            builder: (context, snapshot) {
              if (snapshot.connectionState == ConnectionState.waiting) {
                return Center(
                  child: CircularProgressIndicator(
                    valueColor: AlwaysStoppedAnimation<Color>(AccountInfoScreen.accentColor),
                  ),
                );
              }

              if (snapshot.hasError || !snapshot.hasData) {
                return Center(
                  child: Text(
                    "Lỗi khi lấy thông tin: ${snapshot.error ?? 'Không có dữ liệu'}",
                    style: TextStyle(
                      fontSize: 18,
                      color: AccountInfoScreen.textColor,
                      fontFamily: 'Poppins',
                    ),
                  ),
                );
              }

              final result = snapshot.data!;
              return result.fold(
                (error) => Center(
                  child: Text(
                    "Lỗi: $error",
                    style: TextStyle(
                      fontSize: 18,
                      color: AccountInfoScreen.textColor,
                      fontFamily: 'Poppins',
                    ),
                  ),
                ),
                (userData) {
                  _currentUserData = userData;
                  return Form(
                    key: _formKey,
                    child: ListView(
                      children: [
                        Container(
                          margin: const EdgeInsets.all(20),
                          padding: const EdgeInsets.all(20),
                          decoration: BoxDecoration(
                            color: AccountInfoScreen.primaryColor.withOpacity(0.2),
                            borderRadius: BorderRadius.circular(12),
                          ),
                          child: Column(
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: [
                              Center(
                                child: Stack(
                                  alignment: Alignment.bottomRight,
                                  children: [
                                    CircleAvatar(
                                      radius: 50,
                                      backgroundColor: AccountInfoScreen.accentColor,
                                      child: userData.avatar?.isNotEmpty == true
                                          ? ClipOval(
                                              child: Image.network(
                                                userData.avatar!,
                                                width: 100,
                                                height: 100,
                                                fit: BoxFit.cover,
                                                errorBuilder: (context, error, stackTrace) => const Icon(
                                                  Icons.person,
                                                  size: 60,
                                                  color: Colors.white,
                                                ),
                                              ),
                                            )
                                          : const Icon(
                                              Icons.person,
                                              size: 60,
                                              color: Colors.white,
                                            ),
                                    ),
                                    GestureDetector(
                                      onTap: authProvider.isLoading ? null : () => _pickAndUploadAvatar(context),
                                      child: CircleAvatar(
                                        radius: 15,
                                        backgroundColor: AccountInfoScreen.accentColor,
                                        child: const Icon(
                                          Icons.camera_alt,
                                          size: 16,
                                          color: Colors.white,
                                        ),
                                      ),
                                    ),
                                  ],
                                ),
                              ),
                              const SizedBox(height: 16),
                              _buildSectionTitle("Thông Tin Cá Nhân"),
                              if (!_isEditing)
                                _buildInfoRow("Họ và tên", userData.name)
                              else
                                _buildEditableInfoRow(
                                  label: "Họ và tên",
                                  controller: _fullNameController,
                                  validator: (value) {
                                    if (value == null || value.isEmpty) {
                                      return 'Vui lòng nhập họ và tên';
                                    }
                                    return null;
                                  },
                                ),
                              _buildDivider(),
                              if (!_isEditing)
                                _buildInfoRow("Ngày sinh", _formatDisplayDate(userData.dateOfBirth))
                              else
                                _buildDatePickerRow(
                                  label: "Ngày sinh",
                                  controller: _dateOfBirthController,
                                  validator: (value) {
                                    if (value == null || value.isEmpty) {
                                      return 'Vui lòng nhập ngày sinh';
                                    }
                                    try {
                                      DateFormat('dd-MM-yyyy').parse(value);
                                    } catch (e) {
                                      return 'Định dạng ngày không hợp lệ (dd-MM-yyyy)';
                                    }
                                    return null;
                                  },
                                ),
                              _buildDivider(),
                              if (!_isEditing)
                                _buildInfoRow("Giới tính", userData.gender ?? "Không có dữ liệu")
                              else
                                _buildGenderDropdownRow(
                                  label: "Giới tính",
                                  controller: _genderController,
                                ),
                              _buildSectionTitle("Thông Tin Tài Khoản"),
                              if (!_isEditing)
                                _buildInfoRow("Tên đăng nhập", userData.userName ?? "Không có dữ liệu")
                              else
                                _buildEditableInfoRow(
                                  label: "Tên đăng nhập",
                                  controller: _userNameController,
                                ),
                              _buildDivider(),
                              if (!_isEditing)
                                _buildInfoRow("Email", userData.email ?? "Không có dữ liệu")
                              else
                                _buildEditableInfoRow(
                                  label: "Email",
                                  controller: _emailController,
                                  validator: (value) {
                                    if (value == null || value.isEmpty) {
                                      return 'Vui lòng nhập email';
                                    }
                                    if (!RegExp(r'^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$').hasMatch(value)) {
                                      return 'Email không hợp lệ';
                                    }
                                    return null;
                                  },
                                ),
                              _buildDivider(),
                              if (!_isEditing)
                                _buildInfoRow("Số điện thoại", userData.phoneNumber ?? "Không có dữ liệu")
                              else
                                _buildEditableInfoRow(
                                  label: "Số điện thoại",
                                  controller: _phoneController,
                                  validator: (value) {
                                    if (value == null || value.isEmpty) {
                                      return 'Vui lòng nhập số điện thoại';
                                    }
                                    if (!RegExp(r'^\+?0[0-9]{9,10}$').hasMatch(value)) {
                                      return 'Số điện thoại không hợp lệ';
                                    }
                                    return null;
                                  },
                                ),
                              _buildDivider(),
                              _buildInfoRow(
                                "Xác minh email",
                                userData.isEmailVerified == true ? "Đã xác minh" : "Chưa xác minh",
                              ),
                              if (userData.emailVerificationToken?.isNotEmpty == true) ...[
                                _buildDivider(),
                                _buildInfoRow(
                                  "Mã xác minh email",
                                  userData.emailVerificationToken!,
                                ),
                              ],
                              _buildDivider(),
                              _buildInfoRow(
                                "Loại tài khoản",
                                userData.isEmp == true ? "Nhân viên" : "Khách hàng",
                              ),
                              _buildDivider(),
                              _buildInfoRow(
                                "Vai trò",
                                userData.roleID == 1 ? "Khách hàng" : "Nhân viên",
                              ),
                            ],
                          ),
                        ),
                        _buildSectionTitle("ĐIỂM GIÚP ANH TÌM SALON NHANH NHẤT"),
                        _buildAddressItem(context, Icons.store, "Địa chỉ cơ quan", "Không có dữ liệu"),
                        _buildAddressItem(context, Icons.directions_car, "Nơi hay đi lại", "Không có dữ liệu"),
                        _buildAddressItem(context, Icons.account_balance, "Địa chỉ nhà", "Không có dữ liệu"),
                        _buildSectionTitle("ĐỊA CHỈ NHẬN HÀNG CỦA ANH"),
                        _buildAddressItem(
                          context,
                          Icons.add,
                          "Thêm địa điểm nhận hàng",
                          "Lưu địa điểm nhận hàng của anh",
                          isAddButton: true,
                        ),
                      ],
                    ),
                  );
                },
              );
            },
          ),
          if (authProvider.isLoading)
            Container(
              color: Colors.black.withOpacity(0.5),
              child: Center(
                child: CircularProgressIndicator(
                  valueColor: AlwaysStoppedAnimation<Color>(AccountInfoScreen.accentColor),
                ),
              ),
            ),
        ],
      ),
    );
  }

  Widget _buildSectionTitle(String title) {
    return Padding(
      padding: const EdgeInsets.only(left: 0, top: 20, bottom: 10),
      child: Text(
        title,
        style: const TextStyle(
          fontSize: 16,
          fontWeight: FontWeight.w600,
          color: AccountInfoScreen.textColor,
          fontFamily: 'Poppins',
        ),
      ),
    );
  }

  Widget _buildInfoRow(String label, String value) {
    return Padding(
      padding: const EdgeInsets.symmetric(vertical: 8),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Text(
            label,
            style: const TextStyle(
              fontSize: 14,
              color: AccountInfoScreen.dropdownTextColor,
              fontFamily: 'Poppins',
            ),
          ),
          const SizedBox(height: 4),
          Text(
            value,
            style: const TextStyle(
              fontSize: 16,
              fontWeight: FontWeight.w500,
              color: AccountInfoScreen.textColor,
              fontFamily: 'Poppins',
            ),
            maxLines: 2,
            overflow: TextOverflow.ellipsis,
          ),
        ],
      ),
    );
  }

  Widget _buildEditableInfoRow({
    required String label,
    required TextEditingController controller,
    String? Function(String?)? validator,
  }) {
    return Padding(
      padding: const EdgeInsets.symmetric(vertical: 8),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Text(
            label,
            style: const TextStyle(
              fontSize: 14,
              color: AccountInfoScreen.dropdownTextColor,
              fontFamily: 'Poppins',
            ),
          ),
          const SizedBox(height: 4),
          TextFormField(
            controller: controller,
            enabled: true,
            style: const TextStyle(
              fontSize: 16,
              fontWeight: FontWeight.w500,
              color: AccountInfoScreen.textColor,
              fontFamily: 'Poppins',
            ),
            decoration: InputDecoration(
              filled: true,
              fillColor: AccountInfoScreen.primaryColor.withOpacity(0.3),
              border: OutlineInputBorder(
                borderRadius: BorderRadius.circular(8),
                borderSide: BorderSide.none,
              ),
              contentPadding: const EdgeInsets.symmetric(horizontal: 10, vertical: 8),
            ),
            validator: validator,
          ),
        ],
      ),
    );
  }

  Widget _buildDatePickerRow({
    required String label,
    required TextEditingController controller,
    String? Function(String?)? validator,
  }) {
    return Padding(
      padding: const EdgeInsets.symmetric(vertical: 8),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Text(
            label,
            style: const TextStyle(
              fontSize: 14,
              color: AccountInfoScreen.dropdownTextColor,
              fontFamily: 'Poppins',
            ),
          ),
          const SizedBox(height: 4),
          GestureDetector(
            onTap: () async {
              DateTime? pickedDate = await showDatePicker(
                context: context,
                initialDate: DateTime.now(),
                firstDate: DateTime(1900),
                lastDate: DateTime.now(),
                builder: (context, child) {
                  return Theme(
                    data: ThemeData.dark().copyWith(
                      colorScheme: const ColorScheme.dark(
                        primary: AccountInfoScreen.accentColor,
                        onPrimary: AccountInfoScreen.textColor,
                        surface: AccountInfoScreen.primaryColor,
                        onSurface: AccountInfoScreen.textColor,
                      ),
                      dialogBackgroundColor: AccountInfoScreen.backgroundColor,
                    ),
                    child: child!,
                  );
                },
              );
              if (pickedDate != null) {
                setState(() {
                  controller.text = DateFormat('dd-MM-yyyy').format(pickedDate);
                });
              }
            },
            child: AbsorbPointer(
              child: TextFormField(
                controller: controller,
                enabled: true,
                style: const TextStyle(
                  fontSize: 16,
                  fontWeight: FontWeight.w500,
                  color: AccountInfoScreen.textColor,
                  fontFamily: 'Poppins',
                ),
                decoration: InputDecoration(
                  filled: true,
                  fillColor: AccountInfoScreen.primaryColor.withOpacity(0.3),
                  border: OutlineInputBorder(
                    borderRadius: BorderRadius.circular(8),
                    borderSide: BorderSide.none,
                  ),
                  contentPadding: const EdgeInsets.symmetric(horizontal: 10, vertical: 8),
                  suffixIcon: const Icon(
                    Icons.calendar_today,
                    color: AccountInfoScreen.accentColor,
                  ),
                ),
                validator: validator,
              ),
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildGenderDropdownRow({
    required String label,
    required TextEditingController controller,
  }) {
    const List<String> genderOptions = ['Nam', 'Nữ', 'Khác'];

    return Padding(
      padding: const EdgeInsets.symmetric(vertical: 8),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Text(
            label,
            style: const TextStyle(
              fontSize: 14,
              color: AccountInfoScreen.dropdownTextColor,
              fontFamily: 'Poppins',
            ),
          ),
          const SizedBox(height: 4),
          DropdownButtonFormField<String>(
            value: genderOptions.contains(controller.text) ? controller.text : null,
            decoration: InputDecoration(
              filled: true,
              fillColor: AccountInfoScreen.primaryColor.withOpacity(0.3),
              border: OutlineInputBorder(
                borderRadius: BorderRadius.circular(8),
                borderSide: BorderSide.none,
              ),
              contentPadding: const EdgeInsets.symmetric(horizontal: 10, vertical: 8),
            ),
            dropdownColor: AccountInfoScreen.primaryColor,
            style: const TextStyle(
              fontSize: 16,
              fontWeight: FontWeight.w500,
              color: AccountInfoScreen.textColor,
              fontFamily: 'Poppins',
            ),
            items: genderOptions.map((String gender) {
              return DropdownMenuItem<String>(
                value: gender,
                child: Text(
                  gender,
                  style: const TextStyle(
                    color: AccountInfoScreen.textColor,
                  ),
                ),
              );
            }).toList(),
            onChanged: (String? newValue) {
              if (newValue != null) {
                setState(() {
                  controller.text = newValue;
                });
              }
            },
            validator: (value) {
              if (value == null || value.isEmpty) {
                return 'Vui lòng chọn giới tính';
              }
              return null;
            },
          ),
        ],
      ),
    );
  }

  Widget _buildDivider() {
    return Divider(
      color: AccountInfoScreen.dropdownTextColor.withOpacity(0.3),
      thickness: 1,
      height: 20,
    );
  }

  Widget _buildAddressItem(
    BuildContext context,
    IconData icon,
    String title,
    String subtitle, {
    bool isAddButton = false,
  }) {
    return Card(
      margin: const EdgeInsets.symmetric(horizontal: 20, vertical: 5),
      color: AccountInfoScreen.primaryColor.withOpacity(0.2),
      shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(12)),
      child: ListTile(
        leading: Icon(icon, color: AccountInfoScreen.accentColor),
        title: Text(
          title,
          style: const TextStyle(
            fontSize: 16,
            color: AccountInfoScreen.textColor,
            fontFamily: 'Poppins',
          ),
        ),
        subtitle: Text(
          subtitle,
          style: const TextStyle(
            fontSize: 14,
            color: AccountInfoScreen.dropdownTextColor,
            fontFamily: 'Poppins',
          ),
        ),
        trailing: const Icon(Icons.arrow_forward_ios,
            color: AccountInfoScreen.dropdownTextColor, size: 16),
        onTap: () {
          // TODO: Implement navigation or action for address items
        },
      ),
    );
  }
}