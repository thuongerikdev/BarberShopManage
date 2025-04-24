import 'package:barbermanagemobile/domain/repositories/booking_order_repository.dart';

class PayBookingOrderUseCase {
  final BookingOrderRepository repository;

  PayBookingOrderUseCase(this.repository);

  Future<String> call(int orderID) async {
    return await repository.payBookingOrder(orderID);
  }
}