import 'package:barbermanagemobile/data/datasources/booking_product_remote_data_source.dart';
import 'package:barbermanagemobile/domain/entities/booking_product.dart';
import 'package:barbermanagemobile/domain/repositories/booking_product_repository.dart';

class BookingProductRepositoryImpl implements BookingProductRepository {
  final BookingProductRemoteDataSource remoteDataSource;

  BookingProductRepositoryImpl(this.remoteDataSource);

  @override
  Future<List<BookingProduct>> getBookingProducts() async {
    final models = await remoteDataSource.getBookingProducts();
    return models.map((model) => BookingProduct(
          productID: model.productID,
          productName: model.productName,
          productDescription: model.productDescription,
          productPrice: model.productPrice,
          productImage: model.productImage,
        )).toList();
  }

  @override
  Future<BookingProduct> getBookingProductById(int productId) async {
    final model = await remoteDataSource.getBookingProductById(productId);
    return BookingProduct(
      productID: model.productID,
      productName: model.productName,
      productDescription: model.productDescription,
      productPrice: model.productPrice,
      productImage: model.productImage,
    );
  }

  @override
  Future<List<BookingProduct>> getBookingProductsByCategory(int categoryId) async {
    final models = await remoteDataSource.getBookingProductsByCategory(categoryId);
    return models.map((model) => BookingProduct(
          productID: model.productID,
          productName: model.productName,
          productDescription: model.productDescription,
          productPrice: model.productPrice,
          productImage: model.productImage,
        )).toList();
  }
}