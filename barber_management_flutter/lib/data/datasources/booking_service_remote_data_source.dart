import 'dart:convert';
import 'package:http/http.dart' as http;
import 'package:flutter_dotenv/flutter_dotenv.dart';
import '../models/booking_create_order_request_model.dart';

abstract class BookingServiceRemoteDataSource {
  Future<List<Map<String, dynamic>>> getBookingServices();
  Future<List<Map<String, dynamic>>> getBookingServiceDetail(int serviceID);
  Future<Map<String, dynamic>> getServiceById(int serviceID);
  Future<List<Map<String, dynamic>>> getAllBranches();
  Future<List<Map<String, dynamic>>> getEmployeesByBranch(int branchID);
  Future<List<Map<String, dynamic>>> getEmployeesByDate(DateTime date, int branchID, String typeOfEmp);
  Future<void> createBookingOrder(BookingCreateOrderRequestModel request);
  Future<Map<String, dynamic>> getInvoiceByOrderId(int orderID);
  Future<List<Map<String, dynamic>>> getBookingServiceDetailDescription(int serviceDetailID); // New method
}

class BookingServiceRemoteDataSourceImpl implements BookingServiceRemoteDataSource {
  final String baseUrl = dotenv.get('BASE_URL');

  @override
  Future<List<Map<String, dynamic>>> getBookingServices() async {
    final response = await http.get(
      Uri.parse('$baseUrl/BookingService/getall'),
      headers: {'accept': '*/*'},
    );

    print('BookingService API response: ${response.body}');

    if (response.statusCode == 200) {
      final jsonResponse = jsonDecode(response.body);
      if (jsonResponse['errorCode'] == 200) {
        final data = List<Map<String, dynamic>>.from(jsonResponse['data'] ?? []);
        print('Parsed services: $data');
        return data.map((item) {
          final price = double.tryParse(item['servPrice']?.toString() ?? '0') ?? 0.0;
          return {
            'servName': item['servName']?.toString() ?? 'Dịch vụ không tên',
            'servImage': item['servImage']?.toString() ?? '',
            'servDescription': item['servDescription']?.toString() ?? 'Không có mô tả',
            'servID': item['serviceDetailID']?.toString() ?? item['servID']?.toString() ?? '',
            'servPrice': price,
          };
        }).toList();
      } else {
        throw Exception(jsonResponse['errorMessager'] ?? 'Lấy dịch vụ thất bại');
      }
    } else {
      throw Exception('Lỗi server: ${response.statusCode}');
    }
  }

  @override
  Future<List<Map<String, dynamic>>> getBookingServiceDetail(int serviceID) async {
    final response = await http.get(
      Uri.parse('$baseUrl/BookingServiceDetail/getbyservice/$serviceID'),
      headers: {'accept': '*/*'},
    );

    print('BookingServiceDetail API response: ${response.body}');

    if (response.statusCode == 200) {
      final jsonResponse = jsonDecode(response.body);
      if (jsonResponse['errorCode'] == 200) {
        final data = List<Map<String, dynamic>>.from(jsonResponse['data'] ?? []);
        return data.map((item) {
          final price = double.tryParse(item['servPrice']?.toString() ?? '0') ?? 0.0;
          return {
            'servName': item['servName']?.toString() ?? 'Dịch vụ không tên',
            'servImage': item['servImage']?.toString() ?? '',
            'servDescription': item['servDescription']?.toString() ?? 'Không có mô tả',
            'servID': item['servID']?.toString() ?? '',
            'serviceDetailID': item['serviceDetailID']?.toString() ?? '',
            'servPrice': price,
            'servStatus': item['servStatus']?.toString() ?? 'Unknown',
            'bookingAppointments': item['bookingAppointments'] != null
                ? List<Map<String, dynamic>>.from(item['bookingAppointments'] ?? [])
                : [],
          };
        }).toList();
      } else {
        throw Exception(jsonResponse['errorMessager'] ?? 'Lấy chi tiết dịch vụ thất bại');
      }
    } else {
      throw Exception('Lỗi server: ${response.statusCode}');
    }
  }

  @override
  Future<Map<String, dynamic>> getServiceById(int serviceID) async {
    final response = await http.get(
      Uri.parse('$baseUrl/BookingService/get/$serviceID'),
      headers: {'accept': '*/*'},
    );

    print('BookingService get by ID API response: ${response.body}');

    if (response.statusCode == 200) {
      final jsonResponse = jsonDecode(response.body);
      if (jsonResponse['errorCode'] == 200) {
        final data = jsonResponse['data'] as Map<String, dynamic>;
        return {
          'servID': data['servID']?.toString() ?? '',
          'servName': data['servName']?.toString() ?? 'Dịch vụ không tên',
          'servDescription': data['servDescription']?.toString() ?? 'Không có mô tả',
          'servPrice': double.tryParse(data['servPrice']?.toString() ?? '0') ?? 0.0,
          'servImage': data['servImage']?.toString() ?? '',
          'servStatus': data['servStatus']?.toString() ?? 'Unknown',
        };
      } else {
        throw Exception(jsonResponse['errorMessager'] ?? 'Lấy dịch vụ thất bại');
      }
    } else {
      throw Exception('Lỗi server: ${response.statusCode}');
    }
  }

  @override
  Future<List<Map<String, dynamic>>> getAllBranches() async {
    final response = await http.get(
      Uri.parse('$baseUrl/AuthBranch/getallbranch'),
      headers: {'accept': '*/*'},
    );

    if (response.statusCode == 200) {
      final jsonResponse = jsonDecode(response.body);
      if (jsonResponse['errorCode'] == 200) {
        return List<Map<String, dynamic>>.from(jsonResponse['data']);
      } else {
        throw Exception(jsonResponse['errorMessager'] ?? 'Lấy chi nhánh thất bại');
      }
    } else {
      throw Exception('Lỗi server: ${response.statusCode}');
    }
  }

  @override
  Future<List<Map<String, dynamic>>> getEmployeesByBranch(int branchID) async {
    final response = await http.get(
      Uri.parse('$baseUrl/AuthBranch/getempByBranch/$branchID'),
      headers: {'accept': '*/*'},
    );

    if (response.statusCode == 200) {
      final jsonResponse = jsonDecode(response.body);
      if (jsonResponse['errorCode'] == 200) {
        final List<dynamic> data = jsonResponse['data'] ?? [];
        return data.map((item) {
          final authUser = item['authUser'] ?? {};
          return {
            'empID': item['empID']?.toString() ?? '',
            'empName': authUser['fullName']?.toString() ?? 'Unknown',
            'email': authUser['email']?.toString() ?? '',
            'branchID': item['branchID']?.toString() ?? '',
            'status': item['status']?.toString() ?? 'Unknown',
          };
        }).toList().cast<Map<String, dynamic>>();
      } else {
        throw Exception(jsonResponse['errorMessager'] ?? 'Lấy nhân viên thất bại');
      }
    } else {
      throw Exception('Lỗi server: ${response.statusCode}');
    }
  }

  @override
  Future<List<Map<String, dynamic>>> getEmployeesByDate(DateTime date, int branchID, String typeOfEmp) async {
    final formattedDate = Uri.encodeComponent(date.toIso8601String());
    final response = await http.get(
      Uri.parse('$baseUrl/AuthSchedule/getEmpByDate?date=$formattedDate&branchesID=$branchID&typeOfEmp=$typeOfEmp'),
      headers: {'accept': '*/*'},
    );

    print('getEmployeesByDate API response: ${response.body}');

    if (response.statusCode == 200) {
      final jsonResponse = jsonDecode(response.body);
      if (jsonResponse['errorCode'] == 200) {
        final List<dynamic> data = jsonResponse['data'] ?? [];
        return data.map((item) {
          return {
            'empID': item['employeeId']?.toString() ?? '',
            'empName': item['employeeName']?.toString() ?? 'Unknown',
            'position': item['position']?.toString() ?? '',
            'specialty': item['specialty']?.toString() ?? '',
            'branch': item['branch']?.toString() ?? '',
            'scheduleName': item['scheduleName']?.toString() ?? '',
            'startTime': item['startTime']?.toString() ?? '',
            'endTime': item['endTime']?.toString() ?? '',
            'image' : item['image']?.toString() ?? ''
          };
        }).toList().cast<Map<String, dynamic>>();
      } else {
        throw Exception(jsonResponse['errorMessager'] ?? 'Lấy nhân viên thất bại');
      }
    } else {
      throw Exception('Lỗi server: ${response.statusCode}');
    }
  }

  @override
  Future<void> createBookingOrder(BookingCreateOrderRequestModel request) async {
    final response = await http.post(
      Uri.parse('$baseUrl/BookingBussiness/CreateOrder'),
      headers: {
        'accept': '*/*',
        'Content-Type': 'application/json',
      },
      body: jsonEncode(request.toJson()),
    );

    print('createBookingOrder API response: ${response.body}');

    if (response.statusCode == 200 || response.statusCode == 201) {
      final jsonResponse = jsonDecode(response.body);
      if (jsonResponse['errorCode'] == 200) {
        return;
      } else {
        throw Exception(jsonResponse['errorMessager'] ?? 'Tạo đơn đặt thất bại');
      }
    } else {
      throw Exception('Lỗi server: ${response.statusCode}');
    }
  }

  @override
  Future<Map<String, dynamic>> getInvoiceByOrderId(int orderID) async {
    final response = await http.get(
      Uri.parse('$baseUrl/BookingInvoice/GetInvoiceByOrderID/$orderID'),
      headers: {'accept': '*/*'},
    );

    print('getInvoiceByOrderId API response: ${response.body}');

    if (response.statusCode == 200) {
      final jsonResponse = jsonDecode(response.body);
      if (jsonResponse['errorCode'] == 200) {
        final data = jsonResponse['data'] as Map<String, dynamic>;
        return {
          'invoiceID': data['invoiceID']?.toString() ?? '',
          'invoiceDate': data['invoiceDate']?.toString() ?? '',
          'status': data['status']?.toString() ?? 'Unknown',
          'totalAmount': double.tryParse(data['totalAmount']?.toString() ?? '0') ?? 0.0,
          'orderID': data['orderID']?.toString() ?? '',
          'paymentTerms': data['paymentTerms']?.toString() ?? '',
          'paymentMethod': data['paymentMethod']?.toString() ?? '',
          'order': data['order'] != null
              ? {
                  'orderID': data['order']['orderID']?.toString() ?? '',
                  'custID': data['order']['custID']?.toString() ?? '',
                  'orderStatus': data['order']['orderStatus']?.toString() ?? 'Unknown',
                  'appointments': (data['order']['appointments'] as List<dynamic>?)?.map((appt) => {
                        'appointmentID': appt['appointmentID']?.toString() ?? '',
                        'empID': appt['empID']?.toString() ?? '',
                        'appointmentStatus': appt['appointmentStatus']?.toString() ?? 'Unknown',
                        'service': appt['service']?.toString() ?? '',
                      }).toList() ?? [],
                }
              : null,
        };
      } else {
        throw Exception(jsonResponse['errorMessager'] ?? 'Lấy hóa đơn thất bại');
      }
    } else {
      throw Exception('Lỗi server: ${response.statusCode}');
    }
  }

 
  @override
  Future<List<Map<String, dynamic>>> getBookingServiceDetailDescription(int serviceDetailID) async {
    final response = await http.get(
      Uri.parse('$baseUrl/BookingServiceDetailDescription/GetByServiceDetailID/$serviceDetailID'),
      headers: {'accept': '*/*'},
    );

    print('getBookingServiceDetailDescription API response: ${response.body}');

    if (response.statusCode == 200) {
      final jsonResponse = jsonDecode(response.body);
      if (jsonResponse['errorCode'] == 200) {
        final data = List<Map<String, dynamic>>.from(jsonResponse['data'] ?? []);
        return data.map((item) {
          final price = double.tryParse(item['servPrice']?.toString() ?? '0') ?? 0.0;
          return {
            'serviceDetailID': item['serviceDetailID']?.toString() ?? '',
            'servID': item['servID']?.toString() ?? '',
            'servPrice': price,
            'servName': item['servName']?.toString() ?? 'Dịch vụ không tên',
            'servDescription': item['servDescription']?.toString() ?? 'Không có mô tả',
            'servStatus': item['servStatus']?.toString() ?? 'Unknown',
            'serviceDetailDescriptionID': item['serviceDetailDescriptionID']?.toString() ?? '',
            'servImage': item['servImage']?.toString() ?? '',
            'servDesName': item['servDesName']?.toString() ?? 'Không có tên',
            'servDesDescription': item['servDesDescription']?.toString() ?? 'Không có mô tả',
            'servType': item['servType']?.toString() ?? 'Unknown',
          };
        }).toList();
      } else {
        throw Exception(jsonResponse['errorMessager'] ?? 'Lấy mô tả chi tiết dịch vụ thất bại');
      }
    } else {
      throw Exception('Lỗi server: ${response.statusCode}');
    }
  }
}