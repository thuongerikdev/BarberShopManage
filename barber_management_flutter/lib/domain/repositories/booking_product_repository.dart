import 'package:barbermanagemobile/domain/entities/booking/booking_product.dart';

abstract class BookingProductRepository {
  Future<List<BookingProduct>> getBookingProducts();
  Future<BookingProduct> getBookingProductById(int productID);
  Future<List<BookingProduct>> getBookingProductsByCategory(int categoryID);
}