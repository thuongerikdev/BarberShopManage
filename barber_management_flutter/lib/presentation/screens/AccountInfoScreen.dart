import 'package:flutter/material.dart';
import 'package:get_it/get_it.dart';
import 'package:intl/intl.dart';
import 'package:provider/provider.dart';
import 'package:barbermanagemobile/domain/usecases/get_user_by_id_use_case.dart';
import 'package:barbermanagemobile/presentation/providers/auth_provider.dart';
import 'package:image_picker/image_picker.dart';
import 'dart:io';

class AccountInfoScreen extends StatefulWidget {
  static const primaryColor = Color(0xFF4E342E);
  static const backgroundColor = Color(0xFF212121);
  static const textColor = Color(0xFFEFEBE9);
  static const accentColor = Color(0xFF8D6E63);
  static const dropdownTextColor = Color(0xFFD7CCC8);

  const AccountInfoScreen({super.key});

  @override
  _AccountInfoScreenState createState() => _AccountInfoScreenState();
}

class _AccountInfoScreenState extends State<AccountInfoScreen> {
  final ImagePicker _picker = ImagePicker();

  Future<void> _pickAndUploadAvatar() async {
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
            onPressed: () {
              // TODO: Implement other edit functionality
            },
            child: const Text(
              "Chỉnh sửa",
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
          FutureBuilder(
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
                (userData) => ListView(
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
                                  onTap: authProvider.isLoading ? null : _pickAndUploadAvatar,
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
                          _buildInfoRow("Họ và tên", userData.name),
                          _buildDivider(),
                          _buildInfoRow("Ngày sinh", _formatDate(userData.dateOfBirth)),
                          _buildDivider(),
                          _buildInfoRow("Giới tính", userData.gender ?? "Không có dữ liệu"),
                          _buildSectionTitle("Thông Tin Tài Khoản"),
                          _buildInfoRow("Tên đăng nhập", userData.userName ?? "Không có dữ liệu"),
                          _buildDivider(),
                          _buildInfoRow("Email", userData.email ?? "Không có dữ liệu"),
                          _buildDivider(),
                          _buildInfoRow("Số điện thoại", userData.phoneNumber ?? "Không có dữ liệu"),
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

  String _formatDate(String? date) {
    if (date == null) return "Không có dữ liệu";
    try {
      final parsedDate = DateTime.parse(date);
      return DateFormat('dd-MM-yyyy').format(parsedDate);
    } catch (e) {
      return "Không có dữ liệu";
    }
  }
}