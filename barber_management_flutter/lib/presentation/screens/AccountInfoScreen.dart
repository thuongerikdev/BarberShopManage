import 'package:flutter/material.dart';
import 'package:get_it/get_it.dart';
import 'package:intl/intl.dart';
import 'package:provider/provider.dart';
import 'package:barbermanagemobile/domain/usecases/get_user_by_id_use_case.dart';
import 'package:barbermanagemobile/presentation/providers/auth_provider.dart';

class AccountInfoScreen extends StatelessWidget {
  static const primaryColor = Color(0xFF4E342E);
  static const backgroundColor = Color(0xFF212121);
  static const textColor = Color(0xFFEFEBE9);
  static const accentColor = Color(0xFF8D6E63);
  static const dropdownTextColor = Color(0xFFD7CCC8);

  const AccountInfoScreen({super.key});

  @override
  Widget build(BuildContext context) {
    final authProvider = Provider.of<AuthProvider>(context);
    final user = authProvider.user;

    if (user?.userId == null || int.tryParse(user!.userId) == null) {
      return Scaffold(
        backgroundColor: backgroundColor,
        appBar: AppBar(
          elevation: 0,
          backgroundColor: primaryColor,
          title: Text(
            "THÔNG TIN TÀI KHOẢN",
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
        body: Center(
          child: Text(
            "Không tìm thấy thông tin người dùng",
            style: TextStyle(
              fontSize: 18,
              color: textColor,
              fontFamily: 'Poppins',
            ),
          ),
        ),
      );
    }

    final userIdInt = int.parse(user.userId);
    final getUserByIDUseCase = GetIt.instance<GetUserByIDUseCase>();

    return Scaffold(
      backgroundColor: backgroundColor,
      appBar: AppBar(
        elevation: 0,
        backgroundColor: primaryColor,
        title: Text(
          "THÔNG TIN TÀI KHOẢN",
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
        actions: [
          TextButton(
            onPressed: () {
              // TODO: Implement edit functionality
            },
            child: Text(
              "Chỉnh sửa",
              style: TextStyle(
                fontSize: 16,
                color: accentColor,
                fontFamily: 'Poppins',
                fontWeight: FontWeight.w600,
              ),
            ),
          ),
        ],
      ),
      body: FutureBuilder(
        future: getUserByIDUseCase.call(userIdInt),
        builder: (context, snapshot) {
          if (snapshot.connectionState == ConnectionState.waiting) {
            return Center(
              child: CircularProgressIndicator(
                valueColor: AlwaysStoppedAnimation<Color>(accentColor),
              ),
            );
          }

          if (snapshot.hasError || !snapshot.hasData) {
            return Center(
              child: Text(
                "Lỗi khi lấy thông tin: ${snapshot.error ?? 'Không có dữ liệu'}",
                style: TextStyle(
                  fontSize: 18,
                  color: textColor,
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
                  color: textColor,
                  fontFamily: 'Poppins',
                ),
              ),
            ),
            (userData) => ListView(
              children: [
                Container(
                  margin: EdgeInsets.all(20),
                  padding: EdgeInsets.all(20),
                  decoration: BoxDecoration(
                    color: primaryColor.withOpacity(0.2),
                    borderRadius: BorderRadius.circular(12),
                  ),
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      _buildInfoRow("Họ và tên", userData.name),
                      _buildDivider(),
                      _buildInfoRow("Số điện thoại", userData.phoneNumber ?? "Không có dữ liệu"),
                      _buildDivider(),
                      _buildInfoRow("Email", userData.email ?? "Không có dữ liệu"),
                      _buildDivider(),
                      _buildInfoRow("Ngày sinh", _formatDate(userData.dateOfBirth)),
                    ],
                  ),
                ),
                _buildSectionTitle("ĐIỂM GIÚP ANH TÌM SALON NHANH NHẤT"),
                _buildAddressItem(context, Icons.store, "Địa chỉ cơ quan", "Không có dữ liệu"),
                _buildAddressItem(context, Icons.directions_car, "Nơi hay đi lại", "Không có dữ liệu"),
                _buildAddressItem(context, Icons.account_balance, "Địa chỉ nhà", "Không có dữ liệu"),
                _buildSectionTitle("ĐỊA CHỈ NHẬN HÀNG CỦA ANH"),
                _buildAddressItem(context, Icons.add, "Thêm địa điểm nhận hàng", "Lưu địa điểm nhận hàng của anh", isAddButton: true),
              ],
            ),
          );
        },
      ),
    );
  }

  Widget _buildSectionTitle(String title) {
    return Padding(
      padding: EdgeInsets.only(left: 20, top: 20, bottom: 10),
      child: Text(
        title,
        style: TextStyle(
          fontSize: 16,
          fontWeight: FontWeight.w600,
          color: textColor,
          fontFamily: 'Poppins',
        ),
      ),
    );
  }

  Widget _buildInfoRow(String label, String value) {
    return Padding(
      padding: EdgeInsets.symmetric(vertical: 8),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Text(
            label,
            style: TextStyle(
              fontSize: 14,
              color: dropdownTextColor,
              fontFamily: 'Poppins',
            ),
          ),
          SizedBox(height: 4),
          Text(
            value,
            style: TextStyle(
              fontSize: 16,
              fontWeight: FontWeight.w500,
              color: textColor,
              fontFamily: 'Poppins',
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildDivider() {
    return Divider(
      color: dropdownTextColor.withOpacity(0.3),
      thickness: 1,
      height: 20,
    );
  }

  Widget _buildAddressItem(BuildContext context, IconData icon, String title, String subtitle, {bool isAddButton = false}) {
    return Card(
      margin: EdgeInsets.symmetric(horizontal: 20, vertical: 5),
      color: primaryColor.withOpacity(0.2),
      shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(12)),
      child: ListTile(
        leading: Icon(icon, color: accentColor),
        title: Text(
          title,
          style: TextStyle(
            fontSize: 16,
            color: textColor,
            fontFamily: 'Poppins',
          ),
        ),
        subtitle: Text(
          subtitle,
          style: TextStyle(
            fontSize: 14,
            color: dropdownTextColor,
            fontFamily: 'Poppins',
          ),
        ),
        trailing: Icon(Icons.arrow_forward_ios, color: dropdownTextColor, size: 16),
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