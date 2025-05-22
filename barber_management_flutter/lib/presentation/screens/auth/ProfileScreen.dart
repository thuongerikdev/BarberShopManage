import 'package:flutter/foundation.dart';
import 'package:flutter/material.dart';
import 'package:get_it/get_it.dart';
import 'package:provider/provider.dart';
import 'package:barbermanagemobile/domain/usecases/get_customer_by_user_id_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/get_user_by_id_use_case.dart';
import 'package:barbermanagemobile/presentation/screens/auth/AccountInfoScreen.dart';
import 'package:barbermanagemobile/presentation/screens/booking/BookingHistoryScreen.dart';
import 'package:barbermanagemobile/presentation/screens/auth/LoginScreen.dart';
import 'package:barbermanagemobile/presentation/screens/vip/PromotionScreen.dart';
import 'package:barbermanagemobile/presentation/screens/vip/VipScreen.dart';
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
    final getUserByIDUseCase = GetIt.instance<GetUserByIDUseCase>();

    if (user?.userId == null || int.tryParse(user!.userId) == null) {
      return Scaffold(
        backgroundColor: backgroundColor,
        appBar: AppBar(
          elevation: 0,
          backgroundColor: primaryColor,
          title: const Text(
            "Hồ Sơ",
            style: TextStyle(
              fontFamily: 'Poppins',
              fontWeight: FontWeight.w700,
              fontSize: 24,
              color: dropdownTextColor,
            ),
          ),
          automaticallyImplyLeading: false,
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

    return Scaffold(
      backgroundColor: backgroundColor,
      appBar: AppBar(
        elevation: 0,
        backgroundColor: primaryColor,
        title: const Text(
          "Hồ Sơ",
          style: TextStyle(
            fontFamily: 'Poppins',
            fontWeight: FontWeight.w700,
            fontSize: 24,
            color: dropdownTextColor,
          ),
        ),
        automaticallyImplyLeading: false,
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
                  padding: const EdgeInsets.all(20),
                  color: primaryColor.withOpacity(0.9),
                  child: Column(
                    children: [
                      CircleAvatar(
                        radius: 50,
                        backgroundColor: accentColor,
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
                            : const Icon(Icons.person, size: 60, color: Colors.white),
                      ),
                      const SizedBox(height: 15),
                      Text(
                        userData.name,
                        style: const TextStyle(
                          fontSize: 24,
                          fontWeight: FontWeight.w700,
                          color: textColor,
                          fontFamily: 'Poppins',
                        ),
                      ),
                      const SizedBox(height: 8),
                      Text(
                        userData.email ?? 'Chưa có email',
                        style: const TextStyle(
                          fontSize: 16,
                          color: dropdownTextColor,
                          fontFamily: 'Poppins',
                        ),
                      ),
                      const SizedBox(height: 8),
                      Row(
                        mainAxisAlignment: MainAxisAlignment.center,
                        children: [
                          Text(
                            userData.phoneNumber ?? 'Chưa có số điện thoại',
                            style: const TextStyle(
                              fontSize: 16,
                              color: dropdownTextColor,
                              fontFamily: 'Poppins',
                            ),
                          ),
                          if (userData.isEmailVerified == true) ...[
                            const SizedBox(width: 8),
                            const Icon(Icons.verified, color: accentColor, size: 20),
                          ],
                        ],
                      ),
                    ],
                  ),
                ),
                _buildSectionTitle("Thông Tin & Thành Viên"),
                _buildProfileItem(
                  context,
                  Icons.account_circle,
                  "Thông tin tài khoản",
                  () async {
                    Navigator.push(
                      context,
                      MaterialPageRoute(builder: (context) => const AccountInfoScreen()),
                    );
                  },
                ),
                _buildProfileItem(context, Icons.star, "Vip Member", () async {
                  if (userData.userId.isNotEmpty && int.tryParse(userData.userId) != null) {
                    final userIdInt = int.parse(userData.userId);
                    if (kDebugMode) {
                      print('Navigating to VipScreen with userId: $userIdInt');
                    }
                    Navigator.push(
                      context,
                      MaterialPageRoute(builder: (context) => VipScreen(userId: userIdInt)),
                    );
                  } else {
                    if (kDebugMode) {
                      print('Invalid userId: ${userData.userId}');
                    }
                    ScaffoldMessenger.of(context).showSnackBar(
                      SnackBar(
                        content: const Text("Không tìm thấy thông tin người dùng"),
                        backgroundColor: primaryColor,
                      ),
                    );
                  }
                }),
                _buildProfileItem(context, Icons.local_offer, "Ưu đãi", () async {
                  Navigator.push(
                    context,
                    MaterialPageRoute(builder: (context) => const PromotionScreen()),
                  );
                }),
                _buildSectionTitle("Lịch Sử & Sở Thích"),
                _buildProfileItem(context, Icons.history, "Lịch sử cắt", () async {
                  if (userData.userId.isNotEmpty && int.tryParse(userData.userId) != null) {
                    final userIdInt = int.parse(userData.userId);
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
                      print('Invalid userId: ${userData.userId}');
                    }
                    ScaffoldMessenger.of(context).showSnackBar(
                      SnackBar(
                        content: const Text("Không tìm thấy thông tin người dùng"),
                        backgroundColor: primaryColor,
                      ),
                    );
                  }
                }),
                _buildProfileItem(context, Icons.favorite, "Sở thích phục vụ", () async {}),
                _buildProfileItem(
                  context,
                  Icons.info,
                  "Hiểu biết để phục vụ anh tốt hơn",
                  () async {},
                ),
                _buildSectionTitle("Hỗ Trợ"),
                _buildProfileItem(
                  context,
                  Icons.support_agent,
                  "Thông tin hỗ trợ khách hàng Hệ thống Salon",
                  () async {},
                ),
                Padding(
                  padding: const EdgeInsets.all(20),
                  child: ElevatedButton(
                    onPressed: () {
                      authProvider.logout();
                      Navigator.pushReplacement(
                        context,
                        MaterialPageRoute(builder: (context) =>  LoginScreen()),
                      );
                    },
                    style: ElevatedButton.styleFrom(
                      backgroundColor: accentColor,
                      padding: const EdgeInsets.symmetric(vertical: 15),
                      shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(12)),
                    ),
                    child: const Text(
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
        },
      ),
    );
  }

  Widget _buildSectionTitle(String title) {
    return Padding(
      padding: const EdgeInsets.only(left: 20, top: 20, bottom: 10),
      child: Text(
        title,
        style: const TextStyle(
          fontSize: 20,
          fontWeight: FontWeight.w600,
          color: textColor,
          fontFamily: 'Poppins',
        ),
      ),
    );
  }

  Widget _buildProfileItem(
    BuildContext context,
    IconData icon,
    String title,
    Future<void> Function() onTap,
  ) {
    return Card(
      margin: const EdgeInsets.symmetric(horizontal: 20, vertical: 5),
      color: primaryColor.withOpacity(0.2),
      shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(12)),
      child: ListTile(
        leading: Icon(icon, color: accentColor),
        title: Text(
          title,
          style: const TextStyle(
            fontSize: 16,
            color: dropdownTextColor,
            fontFamily: 'Poppins',
          ),
        ),
        trailing: const Icon(Icons.arrow_forward_ios, color: dropdownTextColor, size: 16),
        onTap: () async {
          await onTap();
        },
      ),
    );
  }
}