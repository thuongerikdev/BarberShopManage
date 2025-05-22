import 'package:flutter/material.dart';

class AddressScreen extends StatelessWidget {
  static const primaryColor = Color(0xFF4E342E);
  static const backgroundColor = Color(0xFF212121);
  static const textColor = Color(0xFFEFEBE9);
  static const accentColor = Color(0xFF8D6E63);
  static const dropdownTextColor = Color(0xFFD7CCC8);

  const AddressScreen({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: backgroundColor,
      appBar: AppBar(
        elevation: 0,
        backgroundColor: primaryColor,
        title: Text(
          "Địa chỉ của anh",
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
          _buildSectionTitle("Địa điểm giúp anh tìm salon nhanh nhất"),
          _buildAddressItem(context, Icons.work, "Địa chỉ cơ quan", () async {}),
          _buildAddressItem(context, Icons.directions, "Nơi hay đi lại", () async {}),
          _buildAddressItem(context, Icons.home, "Địa chỉ nhà", () async {}),
          _buildSectionTitle("Địa chỉ nhân hàng của anh"),
          _buildAddressItem(context, Icons.add, "Thêm địa điểm nhận hàng", () async {
            // TODO: Implement add new address functionality
          }),
          _buildAddressItem(context, Icons.save, "Lưu địa điểm nhận hàng của anh", () async {
            // TODO: Implement save address functionality
          }),
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

  Widget _buildAddressItem(BuildContext context, IconData icon, String title, Future<void> Function() onTap) {
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