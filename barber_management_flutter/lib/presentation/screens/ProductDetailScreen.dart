import 'package:flutter/material.dart';
import 'package:get_it/get_it.dart';
import 'package:barbermanagemobile/domain/usecases/get_booking_product_by_id_use_case.dart';

class ProductDetailScreen extends StatelessWidget {
  final int productID;

  const ProductDetailScreen({super.key, required this.productID});

  static const primaryColor = Color(0xFF4E342E);
  static const backgroundColor = Color(0xFF212121);
  static const textColor = Color(0xFFEFEBE9);
  static const accentColor = Color(0xFF8D6E63);
  static const shadowColor = Color(0xFF3E2723);

  @override
  Widget build(BuildContext context) {
    final getBookingProductByIdUseCase = GetIt.instance<GetBookingProductByIdUseCase>();

    return Scaffold(
      backgroundColor: backgroundColor,
      appBar: AppBar(
        backgroundColor: primaryColor,
        title: Text(
          'Chi tiết sản phẩm',
          style: TextStyle(
            color: textColor,
            fontFamily: 'Poppins',
            fontWeight: FontWeight.bold,
          ),
        ),
        leading: IconButton(
          icon: Icon(Icons.arrow_back, color: textColor),
          onPressed: () => Navigator.pop(context),
        ),
      ),
      body: FutureBuilder<Map<String, dynamic>>(
        future: getBookingProductByIdUseCase.call(productID),
        builder: (context, snapshot) {
          if (snapshot.connectionState == ConnectionState.waiting) {
            return Center(
              child: CircularProgressIndicator(
                valueColor: AlwaysStoppedAnimation<Color>(accentColor),
              ),
            );
          } else if (snapshot.hasError) {
            return Center(
              child: Text(
                'Lỗi: ${snapshot.error}',
                style: TextStyle(color: Colors.redAccent, fontSize: 14),
              ),
            );
          } else if (!snapshot.hasData) {
            return Center(
              child: Text(
                'Không có dữ liệu sản phẩm',
                style: TextStyle(color: textColor, fontSize: 14),
              ),
            );
          }

          final product = snapshot.data!;
          final productImage = product['productImage']?.toString() ?? 'https://via.placeholder.com/120';
          final productName = product['productName']?.toString() ?? 'Sản phẩm không tên';
          final productDescription = product['productDescription']?.toString() ?? 'Không có mô tả';
          final productPrice = product['productPrice']?.toStringAsFixed(0) ?? '0';

          return SingleChildScrollView(
            physics: BouncingScrollPhysics(),
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Container(
                  height: 300,
                  width: double.infinity,
                  child: Image.network(
                    productImage,
                    fit: BoxFit.cover,
                    loadingBuilder: (context, child, loadingProgress) {
                      if (loadingProgress == null) return child;
                      return Center(
                        child: CircularProgressIndicator(
                          valueColor: AlwaysStoppedAnimation<Color>(accentColor),
                        ),
                      );
                    },
                    errorBuilder: (context, error, stackTrace) => Icon(
                      Icons.broken_image,
                      color: Colors.redAccent,
                      size: 50,
                    ),
                  ),
                ),
                Padding(
                  padding: const EdgeInsets.all(16.0),
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Text(
                        productName,
                        style: TextStyle(
                          fontSize: 24,
                          fontWeight: FontWeight.bold,
                          color: textColor,
                          fontFamily: 'Poppins',
                        ),
                      ),
                      SizedBox(height: 8),
                      Text(
                        '$productPrice VNĐ',
                        style: TextStyle(
                          fontSize: 20,
                          color: accentColor,
                          fontFamily: 'Poppins',
                          fontWeight: FontWeight.w500,
                        ),
                      ),
                      SizedBox(height: 16),
                      Text(
                        'Mô tả sản phẩm',
                        style: TextStyle(
                          fontSize: 18,
                          fontWeight: FontWeight.bold,
                          color: accentColor,
                          fontFamily: 'Poppins',
                        ),
                      ),
                      SizedBox(height: 8),
                      Text(
                        productDescription,
                        style: TextStyle(
                          fontSize: 14,
                          color: textColor,
                          fontFamily: 'Poppins',
                        ),
                      ),
                    ],
                  ),
                ),
              ],
            ),
          );
        },
      ),
    );
  }
}