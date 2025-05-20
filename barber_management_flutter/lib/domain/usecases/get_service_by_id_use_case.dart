import 'package:barbermanagemobile/domain/repositories/booking_service_repository.dart';

class GetServiceByIdUseCase {
  final BookingServiceRepository repository;

  GetServiceByIdUseCase(this.repository);

  Future<Map<String, dynamic>> call(int serviceID) async {
    try {
      return await repository.getServiceById(serviceID);
    } catch (e) {
      throw Exception('Failed to fetch service by ID: $e');
    }
  }
}