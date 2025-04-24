import 'package:barbermanagemobile/data/datasources/booking_service_remote_data_source.dart';
import 'package:barbermanagemobile/domain/entities/booking_create_order.dart';
import 'package:barbermanagemobile/domain/repositories/booking_service_repository.dart';
import 'package:barbermanagemobile/data/models/booking_create_order_request_model.dart';

class BookingServiceRepositoryImpl implements BookingServiceRepository {
  final BookingServiceRemoteDataSource dataSource;

  BookingServiceRepositoryImpl(this.dataSource);

  @override
  Future<List<Map<String, dynamic>>> getAllBranches() async {
    return await dataSource.getAllBranches();
  }

  @override
  Future<List<Map<String, dynamic>>> getEmployeesByBranch(int branchID) async {
    return await dataSource.getEmployeesByBranch(branchID);
  }

  @override
  Future<List<Map<String, dynamic>>> getEmployeesByDate(DateTime date, int branchID) async {
    return await dataSource.getEmployeesByDate(date, branchID);
  }

  @override
  Future<List<Map<String, dynamic>>> getAllServices() async {
    return await dataSource.getBookingServices();
  }

  @override
  Future<List<Map<String, dynamic>>> getBookingServiceDetail(int serviceID) async {
    return await dataSource.getBookingServiceDetail(serviceID);
  }

  @override
  Future<void> createBookingOrder(BookingCreateOrder order) async {
    final requestModel = _mapToRequestModel(order);
    return await dataSource.createBookingOrder(requestModel);
  }

  BookingCreateOrderRequestModel _mapToRequestModel(BookingCreateOrder order) {
    return BookingCreateOrderRequestModel(
      appoint: order.appoint?.map((appoint) => BookingCreateBusinessAppointModel(
            servID: appoint.servID,
            empID: appoint.empID,
            appStatus: appoint.appStatus,
          )).toList(),
      order: order.order != null
          ? BookingCreateOrderBusinessModel(
              custID: order.order!.custID,
              createAt: order.order!.createAt,
              orderDate: order.order!.orderDate,
            )
          : null,
      promoID: order.promoID,
    );
  }
}