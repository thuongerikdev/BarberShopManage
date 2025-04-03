import 'package:barbermanagemobile/domain/repositories/booking_service_repository.dart';

class GetBookingServicesUseCase {
  final BookingServiceRepository repository;

  GetBookingServicesUseCase(this.repository);

  Future<List<Map<String, dynamic>>> call() async {
    return await repository.getAllServices();
  }
}