import 'package:barbermanagemobile/domain/repositories/booking_service_repository.dart';

class GetInvoiceByOrderIdUseCase {
  final BookingServiceRepository repository;

  GetInvoiceByOrderIdUseCase(this.repository);

  Future<Map<String, dynamic>> call(int orderID) async {
    return await repository.getInvoiceByOrderId(orderID);
  }
}