import 'package:barbermanagemobile/data/datasources/booking_service_remote_data_source.dart';
import 'package:barbermanagemobile/domain/repositories/booking_service_repository.dart';

class BookingServiceRepositoryImpl implements BookingServiceRepository {
  final BookingServiceRemoteDataSource remoteDataSource;

  BookingServiceRepositoryImpl(this.remoteDataSource);

  @override
  Future<List<Map<String, dynamic>>> getAllServices() async {
    try {
      return await remoteDataSource.getAllServices();
    } catch (e) {
      throw Exception('Failed to fetch services: $e');
    }
  }
}