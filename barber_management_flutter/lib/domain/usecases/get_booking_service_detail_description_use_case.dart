import 'package:barbermanagemobile/domain/repositories/booking_service_repository.dart';

class GetBookingServiceDetailDescriptionUseCase {
  final BookingServiceRepository repository;

  GetBookingServiceDetailDescriptionUseCase(this.repository);

  Future<List<Map<String, dynamic>>> call(int serviceDetailID) async {
    return await repository.getBookingServiceDetailDescription(serviceDetailID);
  }
}