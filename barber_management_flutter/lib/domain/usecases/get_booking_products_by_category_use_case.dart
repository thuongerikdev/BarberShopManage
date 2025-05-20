import 'package:barbermanagemobile/domain/repositories/booking_product_repository.dart';

class GetBookingProductsByCategoryUseCase {
  final BookingProductRepository repository;

  GetBookingProductsByCategoryUseCase(this.repository);

  Future<List<Map<String, dynamic>>> call(int categoryID) async {
    final products = await repository.getBookingProductsByCategory(categoryID);
    return products.map((product) => {
          'productID': product.productID,
          'productName': product.productName,
          'productPrice': product.productPrice,
          'productImage': product.productImage.isNotEmpty ? product.productImage : 'https://via.placeholder.com/120',
          'productDescription': product.productDescription,
        }).toList();
  }
}