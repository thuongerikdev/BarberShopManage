import 'dart:convert';
import 'package:barbermanagemobile/data/models/booking_create_order_request_model.dart';
import 'package:http/http.dart' as http;
import 'package:flutter_dotenv/flutter_dotenv.dart';
import 'package:barbermanagemobile/data/models/booking_order_model.dart';
import 'package:barbermanagemobile/domain/entities/booking_create_order.dart';

abstract class BookingOrderRemoteDataSource {
  Future<List<BookingOrderModel>> getOrdersByCustomer(int custID);
  Future<void> createBookingOrder(BookingCreateOrder order);
  Future<void> deleteBookingOrder(int orderID);
  Future<String> payBookingOrder(int orderID);
}

class BookingOrderRemoteDataSourceImpl implements BookingOrderRemoteDataSource {
  final String baseUrl = dotenv.get('BASE_URL');

  @override
  Future<List<BookingOrderModel>> getOrdersByCustomer(int custID) async {
    final response = await http.get(
      Uri.parse('$baseUrl/BookingOrder/getbycustomer/$custID'),
      headers: {'accept': '*/*'},
    );

    if (response.statusCode == 200) {
      final jsonData = jsonDecode(response.body);
      if (jsonData['errorCode'] == 200) {
        return List<Map<String, dynamic>>.from(jsonData['data'])
            .map((json) => BookingOrderModel.fromJson(json))
            .toList();
      } else {
        throw Exception(
            jsonData['errorMessager'] ?? 'Lấy danh sách đơn hàng thất bại');
      }
    } else {
      throw Exception('Lỗi server: ${response.statusCode}');
    }
  }

  @override
  Future<void> createBookingOrder(BookingCreateOrder order) async {
    // Convert BookingCreateOrder to BookingCreateOrderRequestModel
    final requestModel = BookingCreateOrderRequestModel(
      appoint: order.appoint
          ?.map((a) => BookingCreateBusinessAppointModel(
                servID: a.servID,
                empID: a.empID,
                appStatus: a.appStatus,
              ))
          .toList(),
      order: order.order != null
          ? BookingCreateOrderBusinessModel(
              custID: order.order!.custID,
              createAt: order.order!.createAt,
              orderDate: order.order!.orderDate,
            )
          : null,
      promoID: order.promoID,
    );

    final response = await http.post(
      Uri.parse('$baseUrl/BookingBussiness/CreateOrder'),
      headers: {
        'accept': '*/*',
        'Content-Type': 'application/json',
      },
      body: jsonEncode(requestModel.toJson()),
    );

    if (response.statusCode == 200) {
      final jsonData = jsonDecode(response.body);
      if (jsonData['errorCode'] != 200) {
        throw Exception(jsonData['errorMessager'] ?? 'Tạo đơn hàng thất bại');
      }
    } else {
      throw Exception('Lỗi server: ${response.statusCode}');
    }
  }

  @override
  Future<void> deleteBookingOrder(int orderID) async {
    final response = await http.delete(
      Uri.parse('$baseUrl/BookingOrder/delete/$orderID'),
      headers: {'accept': '*/*'},
    );

    if (response.statusCode == 200) {
      final jsonData = jsonDecode(response.body);
      if (jsonData['errorCode'] != 200) {
        throw Exception(jsonData['errorMessager'] ?? 'Xóa đơn hàng thất bại');
      }
    } else {
      throw Exception('Lỗi server: ${response.statusCode}');
    }
  }

  @override
  Future<String> payBookingOrder(int orderID) async {
    // print("oderID >>>>>>> :  $orderID");
    // print('$baseUrl/BookingBussiness/vnpay/payment?orderID=$orderID');
    final response = await http.post(
      Uri.parse('$baseUrl/BookingBussiness/vnpay/payment?orderID=$orderID'),
      headers: {'accept': '*/*'},
    );

    if (response.statusCode == 200) {
      final jsonData = jsonDecode(response.body);
      if (jsonData['payUrl'] != null && jsonData['payUrl']['result'] != null) {
        return jsonData['payUrl']['result'] as String;
      } else {
        throw Exception('Không thể lấy URL thanh toán');
      }
    } else {
      throw Exception('Lỗi server: ${response.statusCode}');
    }
  }
}
