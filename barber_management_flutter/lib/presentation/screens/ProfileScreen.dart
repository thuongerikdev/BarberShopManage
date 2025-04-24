import 'package:flutter/foundation.dart';
import 'package:flutter/material.dart';
import 'package:get_it/get_it.dart';
import 'package:provider/provider.dart';
import 'package:barbermanagemobile/domain/usecases/get_customer_by_user_id_use_case.dart';
import 'package:barbermanagemobile/presentation/screens/AccountInfoScreen.dart';
import 'package:barbermanagemobile/presentation/screens/AddressScreen.dart'; // Add this import
import 'package:barbermanagemobile/presentation/screens/BookingHistoryScreen.dart';
import 'package:barbermanagemobile/presentation/screens/LoginScreen.dart';
import 'package:barbermanagemobile/presentation/screens/PromotionScreen.dart';
import 'package:barbermanagemobile/presentation/screens/VipScreen.dart';
import 'package:barbermanagemobile/presentation/providers/auth_provider.dart';

class ProfileScreen extends StatelessWidget {
  static const primaryColor = Color(0xFF4E342E);
  static const backgroundColor = Color(0xFF212121);
  static const textColor = Color(0xFFEFEBE9);
  static const accentColor = Color(0xFF8D6E63);
  static const dropdownTextColor = Color(0xFFD7CCC8);

  const ProfileScreen({super.key});

  @override
  Widget build(BuildContext context) {
    final authProvider = Provider.of<AuthProvider>(context);
    final user = authProvider.user;

    return Scaffold(
      backgroundColor: backgroundColor,
      appBar: AppBar(
        elevation: 0,
        backgroundColor: primaryColor,
        title: Text(
          "Hồ Sơ",
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
      body: ListView(
        children: [
          Container(
            padding: EdgeInsets.all(20),
            color: primaryColor.withOpacity(0.9),
            child: Column(
              children: [
                CircleAvatar(
                  radius: 50,
                  backgroundColor: accentColor,
                  child: Icon(Icons.person, size: 60, color: Colors.white),
                ),
                SizedBox(height: 15),
                Text(
                  "Chào ${user?.name ?? 'Người dùng'}",
                  style: TextStyle(
                    fontSize: 24,
                    fontWeight: FontWeight.w700,
                    color: textColor,
                    fontFamily: 'Poppins',
                  ),
                ),
              ],
            ),
          ),
          _buildSectionTitle("Thông Tin Cá Nhân"),
          _buildProfileItem(context, Icons.account_circle, "Thông tin tài khoản", () async {
            Navigator.push(
              context,
              MaterialPageRoute(builder: (context) => AccountInfoScreen()),
            );
          }),
          _buildProfileItem(context, Icons.location_on, "Địa chỉ", () async {
            Navigator.push(
              context,
              MaterialPageRoute(builder: (context) => AddressScreen()),
            );
          }),
          _buildProfileItem(context, Icons.shopping_cart, "Đơn hàng", () async {}),
          _buildSectionTitle("Thành Viên & Ưu Đãi"),
          _buildProfileItem(context, Icons.star, "Vip Member", () async {
            Navigator.push(
              context,
              MaterialPageRoute(builder: (context) => VipScreen()),
            );
          }),
          _buildProfileItem(context, Icons.local_offer, "Ưu đãi", () async {
            Navigator.push(
              context,
              MaterialPageRoute(builder: (context) => PromotionScreen()),
            );
          }),
          _buildSectionTitle("Lịch Sử & Sở Thích"),
          _buildProfileItem(context, Icons.history, "Lịch sử cắt", () async {
            if (user?.userId != null && int.tryParse(user!.userId) != null) {
              final userIdInt = int.parse(user.userId);
              if (kDebugMode) {
                print('Navigating to BookingHistoryScreen with userId: $userIdInt');
              }
              try {
                final getCustomerByUserIDUseCase = GetIt.instance<GetCustomerByUserIDUseCase>();
                final result = await getCustomerByUserIDUseCase.call(userIdInt);
                result.fold(
                  (error) => ScaffoldMessenger.of(context).showSnackBar(
                    SnackBar(
                      content: Text("Lỗi: $error"),
                      backgroundColor: primaryColor,
                    ),
                  ),
                  (customer) {
                    if (kDebugMode) {
                      print('Navigating to BookingHistoryScreen with custID: ${customer.customerID}');
                    }
                    Navigator.push(
                      context,
                      MaterialPageRoute(
                        builder: (context) => BookingHistoryScreen(custID: customer.customerID),
                      ),
                    );
                  },
                );
              } catch (e) {
                if (kDebugMode) {
                  print('Error fetching customer: $e');
                }
                ScaffoldMessenger.of(context).showSnackBar(
                  SnackBar(
                    content: Text("Lỗi khi lấy thông tin khách hàng: $e"),
                    backgroundColor: primaryColor,
                  ),
                );
              }
            } else {
              if (kDebugMode) {
                print('Invalid userId: ${user?.userId}');
              }
              ScaffoldMessenger.of(context).showSnackBar(
                SnackBar(
                  content: Text("Không tìm thấy thông tin người dùng"),
                  backgroundColor: primaryColor,
                ),
              );
            }
          }),
          _buildProfileItem(context, Icons.favorite, "Sở thích phục vụ", () async {}),
          _buildProfileItem(context, Icons.info, "Hiểu biết để phục vụ anh tốt hơn", () async {}),
          _buildSectionTitle("Hỗ Trợ"),
          _buildProfileItem(context, Icons.support_agent, "Thông tin hỗ trợ khách hàng Hệ thống Salon", () async {}),
          Padding(
            padding: EdgeInsets.all(20),
            child: ElevatedButton(
              onPressed: () {
                authProvider.logout();
                Navigator.pushReplacement(
                  context,
                  MaterialPageRoute(builder: (context) => LoginScreen()),
                );
              },
              style: ElevatedButton.styleFrom(
                backgroundColor: accentColor,
                padding: EdgeInsets.symmetric(vertical: 15),
                shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(12)),
              ),
              child: Text(
                "Đăng xuất",
                style: TextStyle(
                  fontSize: 18,
                  fontWeight: FontWeight.w600,
                  color: Colors.white,
                  fontFamily: 'Poppins',
                ),
              ),
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildSectionTitle(String title) {
    return Padding(
      padding: EdgeInsets.only(left: 20, top: 20, bottom: 10),
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

  Widget _buildProfileItem(BuildContext context, IconData icon, String title, Future<void> Function() onTap) {
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
            color: dropdownTextColor,
            fontFamily: 'Poppins',
          ),
        ),
        trailing: Icon(Icons.arrow_forward_ios, color: dropdownTextColor, size: 16),
        onTap: () async {
          await onTap();
        },
      ),
    );
  }
}