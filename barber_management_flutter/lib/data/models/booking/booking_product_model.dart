import 'package:barbermanagemobile/domain/entities/booking/booking_product.dart';

class BookingProductModel extends BookingProduct {
  BookingProductModel({
    int? productID,
    required String productName,
    required String productDescription,
    required double productPrice,
    required String productImage,
  }) : super(
          productID: productID,
          productName: productName,
          productDescription: productDescription,
          productPrice: productPrice,
          productImage: productImage,
        );

  factory BookingProductModel.fromJson(Map<String, dynamic> json) {
    return BookingProductModel(
      productID: json['productID'] as int? ?? 0,
      productName: json['productName'] as String,
      productDescription: json['productDescription'] as String,
      productPrice: (json['productPrice'] as num).toDouble(),
      productImage: json['productImage'] as String,
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'productID': productID,
      'productName': productName,
      'productDescription': productDescription,
      'productPrice': productPrice,
      'productImage': productImage,
    };
  }
}