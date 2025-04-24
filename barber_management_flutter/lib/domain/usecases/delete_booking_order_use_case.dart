import 'package:barbermanagemobile/domain/repositories/booking_order_repository.dart';

class DeleteBookingOrderUseCase {
  final BookingOrderRepository repository;

  DeleteBookingOrderUseCase(this.repository);

  Future<void> call(int orderID) async {
    await repository.deleteBookingOrder(orderID);
  }
}