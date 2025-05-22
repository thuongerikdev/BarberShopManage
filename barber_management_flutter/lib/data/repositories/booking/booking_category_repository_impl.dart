import 'package:barbermanagemobile/data/datasources/booking/booking_category_remote_data_source.dart';
import 'package:barbermanagemobile/domain/repositories/booking_category_repository.dart';

class BookingCategoryRepositoryImpl implements BookingCategoryRepository {
  final BookingCategoryRemoteDataSource remoteDataSource;

  BookingCategoryRepositoryImpl(this.remoteDataSource);

  @override
  Future<List<Map<String, dynamic>>> getBookingCategories() async {
    return await remoteDataSource.getBookingCategories();
  }
}