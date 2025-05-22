class BookingProduct {
  final int? productID;
  final String productName;
  final String productDescription;
  final double productPrice;
  final String productImage;

  BookingProduct({
    this.productID,
    required this.productName,
    required this.productDescription,
    required this.productPrice,
    required this.productImage,
  });

  factory BookingProduct.fromJson(Map<String, dynamic> json) {
    return BookingProduct(
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