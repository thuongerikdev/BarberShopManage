import 'package:barbermanagemobile/domain/repositories/booking_service_repository.dart';

class GetBookingServiceDetailUseCase {
  final BookingServiceRepository _repository;

  GetBookingServiceDetailUseCase(this._repository);

  Future<List<Map<String, dynamic>>> call(int serviceID) async {
    return await _repository.getBookingServiceDetail(serviceID);
  }
}