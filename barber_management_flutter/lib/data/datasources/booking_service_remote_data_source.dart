import 'dart:convert';
import 'package:http/http.dart' as http;
import 'package:flutter_dotenv/flutter_dotenv.dart';
import '../models/booking_create_order_request_model.dart';

class BookingServiceRemoteDataSource {
  final String baseUrl = dotenv.get('BASE_URL');

  Future<List<Map<String, dynamic>>> getAllBranches() async {
    final response = await http.get(
      Uri.parse('$baseUrl/AuthBranch/getallbranch'),
      headers: {'accept': '*/*'},
    );

    if (response.statusCode == 200) {
      final jsonResponse = jsonDecode(response.body);
      if (jsonResponse['errorCode'] == 0) {
        return List<Map<String, dynamic>>.from(jsonResponse['data']);
      } else {
        throw Exception(jsonResponse['errorMessager']);
      }
    } else {
      throw Exception('Failed to load branches: ${response.statusCode}');
    }
  }

  Future<List<Map<String, dynamic>>> getEmployeesByBranch(int branchID) async {
    final response = await http.get(
      Uri.parse('$baseUrl/AuthBranch/getempByBranch/$branchID'),
      headers: {'accept': '*/*'},
    );

    if (response.statusCode == 200) {
      final jsonResponse = jsonDecode(response.body);
      if (jsonResponse['errorCode'] == 0) {
        return List<Map<String, dynamic>>.from(jsonResponse['data']);
      } else {
        throw Exception(jsonResponse['errorMessager']);
      }
    } else {
      throw Exception('Failed to load employees: ${response.statusCode}');
    }
  }

  Future<List<Map<String, dynamic>>> getEmployeesByDate(DateTime date) async {
    final formattedDate = Uri.encodeComponent(date.toIso8601String());
    final response = await http.get(
      Uri.parse('$baseUrl/AuthSchedule/getEmpByDate?date=$formattedDate'),
      headers: {'accept': '*/*'},
    );

    if (response.statusCode == 200) {
      final jsonResponse = jsonDecode(response.body);
      if (jsonResponse['errorCode'] == 0) {
        return List<Map<String, dynamic>>.from(jsonResponse['data']);
      } else {
        throw Exception(jsonResponse['errorMessager']);
      }
    } else {
      throw Exception('Failed to load employees by date: ${response.statusCode}');
    }
  }

  Future<List<Map<String, dynamic>>> getAllServices() async {
    final response = await http.get(
      Uri.parse('$baseUrl/BookingService/getall'),
      headers: {'accept': '*/*'},
    );

    if (response.statusCode == 200) {
      final jsonResponse = jsonDecode(response.body);
      if (jsonResponse['errorCode'] == 0) {
        return List<Map<String, dynamic>>.from(jsonResponse['data']);
      } else {
        throw Exception(jsonResponse['errorMessager']);
      }
    } else {
      throw Exception('Failed to load services: ${response.statusCode}');
    }
  }

  Future<void> createBookingOrder(BookingCreateOrderRequestModel request) async {
    final response = await http.post(
      Uri.parse('$baseUrl/BookingService/create'),
      headers: {
        'accept': '*/*',
        'Content-Type': 'application/json',
      },
      body: jsonEncode(request.toJson()),
    );

    if (response.statusCode == 200 || response.statusCode == 201) {
      final jsonResponse = jsonDecode(response.body);
      if (jsonResponse['errorCode'] == 0) {
        return;
      } else {
        throw Exception(jsonResponse['errorMessager']);
      }
    } else {
      throw Exception('Failed to create booking order: ${response.statusCode}');
    }
  }
}