import 'dart:convert';
import 'package:http/http.dart' as http;
import 'package:flutter_dotenv/flutter_dotenv.dart';
import 'dart:convert';

class BookingServiceRemoteDataSource {

  final String baseUrl = dotenv.get('BASE_URL'); 

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
}