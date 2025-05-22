
import 'package:barbermanagemobile/data/datasources/booking/booking_service_remote_data_source.dart';
import 'package:barbermanagemobile/domain/entities/booking/booking_create_order.dart';
import 'package:barbermanagemobile/data/models/booking/booking_create_order_request_model.dart';
import 'package:barbermanagemobile/domain/repositories/booking_service_repository.dart';

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
  Future<List<Map<String, dynamic>>> getEmployeesByDate(DateTime date, int branchID, String typeOfEmp) async {
    return await dataSource.getEmployeesByDate(date, branchID, typeOfEmp);
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
  Future<Map<String, dynamic>> getServiceById(int serviceID) async {
    return await dataSource.getServiceById(serviceID);
  }

  @override
  Future<void> createBookingOrder(BookingCreateOrder order) async {
    final requestModel = _mapToRequestModel(order);
    return await dataSource.createBookingOrder(requestModel);
  }

  @override
  Future<Map<String, dynamic>> getInvoiceByOrderId(int orderID) async {
    return await dataSource.getInvoiceByOrderId(orderID);
  }

  @override
  Future<List<Map<String, dynamic>>> getBookingServiceDetailDescription(int serviceDetailID) async {
    return await dataSource.getBookingServiceDetailDescription(serviceDetailID);
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