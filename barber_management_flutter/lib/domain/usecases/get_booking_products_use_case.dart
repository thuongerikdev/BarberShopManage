import 'package:barbermanagemobile/domain/repositories/booking_product_repository.dart';

class GetBookingProductsUseCase {
  final BookingProductRepository repository;

  GetBookingProductsUseCase(this.repository);

  Future<List<Map<String, dynamic>>> call() async {
    final products = await repository.getBookingProducts();
    return products.map((product) => {
          'productID': product.productID, // Changed to productID
          'productName': product.productName,
          'productPrice': product.productPrice,
          'productImage': product.productImage.isNotEmpty ? product.productImage : 'https://via.placeholder.com/120',
        }).toList();
  }
}