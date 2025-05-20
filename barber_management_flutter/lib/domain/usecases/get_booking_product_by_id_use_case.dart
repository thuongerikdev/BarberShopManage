import 'package:barbermanagemobile/domain/repositories/booking_product_repository.dart';

class GetBookingProductByIdUseCase {
  final BookingProductRepository repository;

  GetBookingProductByIdUseCase(this.repository);

  Future<Map<String, dynamic>> call(int productID) async {
    final product = await repository.getBookingProductById(productID);
    return {
      'productID': product.productID, // Changed to productID
      'productName': product.productName,
      'productDescription': product.productDescription,
      'productPrice': product.productPrice,
      'productImage': product.productImage.isNotEmpty ? product.productImage : 'https://via.placeholder.com/120',
    };
  }
}