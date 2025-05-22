import 'dart:convert';
import 'package:http/http.dart' as http;
import 'package:flutter_dotenv/flutter_dotenv.dart';

abstract class CheckInRemoteDataSource {
  Future<Map<String, dynamic>> createCheckIn({
    required int customerID,
    required String checkInDate,
    required String checkInStatus,
    required String checkInType,
  });

  Future<List<Map<String, dynamic>>> getCheckInHistory(int customerID); // New method
}

class CheckInRemoteDataSourceImpl implements CheckInRemoteDataSource {
  final String baseUrl = dotenv.get('BASE_URL');

  @override
  Future<Map<String, dynamic>> createCheckIn({
    required int customerID,
    required String checkInDate,
    required String checkInStatus,
    required String checkInType,
  }) async {
    final response = await http.post(
      Uri.parse('$baseUrl/AuthCustomerCheckIn/Create'),
      headers: {
        'accept': '*/*',
        'Content-Type': 'application/json',
      },
      body: jsonEncode({
        'customerID': customerID,
        'checkInDate': checkInDate,
        'checkInStatus': checkInStatus,
        'checkInType': checkInType,
      }),
    );

    if (response.statusCode == 200) {
      final jsonResponse = jsonDecode(response.body);
      if (jsonResponse['errorCode'] == 200) {
        return jsonResponse['data'];
      } else {
        throw Exception(jsonResponse['errorMessager'] ?? 'Check-in thất bại');
      }
    } else {
      throw Exception('Lỗi server: ${response.statusCode}');
    }
  }

  @override
  Future<List<Map<String, dynamic>>> getCheckInHistory(int customerID) async {
    final response = await http.get(
      Uri.parse('$baseUrl/AuthCustomerCheckIn/GetByCustomerId/$customerID'),
      headers: {'accept': '*/*'},
    );

    if (response.statusCode == 200) {
      final jsonResponse = jsonDecode(response.body);
      if (jsonResponse['errorCode'] == 200) {
        return List<Map<String, dynamic>>.from(jsonResponse['data']);
      } else {
        throw Exception(
            jsonResponse['errorMessager'] ?? 'Lấy lịch sử điểm danh thất bại');
      }
    } else {
      throw Exception('Lỗi server: ${response.statusCode}');
    }
  }
}