import 'dart:convert';
import 'package:http/http.dart' as http;
import 'package:flutter_dotenv/flutter_dotenv.dart';
import 'package:flutter/foundation.dart';
import '../models/vip_model.dart';

abstract class VipRemoteDataSource {
  Future<List<VipModel>> getVips();
}

class VipRemoteDataSourceImpl implements VipRemoteDataSource {
  final String baseUrl = dotenv.get('BASE_URL');

  @override
  Future<List<VipModel>> getVips() async {
    try {
      final response = await http.get(
        Uri.parse('$baseUrl/AuthVip/getallvip'),
        headers: {'accept': '*/*'},
      );

      if (response.statusCode == 200) {
        final jsonData = jsonDecode(response.body);
        if (jsonData['errorCode'] == 200) {
          final List<dynamic> data = jsonData['data'] ?? [];
          return data.map((json) => VipModel.fromJson(json as Map<String, dynamic>)).toList();
        } else {
          final errorMessage = jsonData['errorMessager'] ?? 'Lấy danh sách VIP thất bại';
          if (kDebugMode) {
            print('API Error: $errorMessage');
          }
          throw Exception(errorMessage);
        }
      } else {
        if (kDebugMode) {
          print('Server Error: Status Code ${response.statusCode}, Body: ${response.body}');
        }
        throw Exception('Lỗi server: ${response.statusCode}');
      }
    } catch (e) {
      if (kDebugMode) {
        print('Error fetching VIPs: $e');
      }
      throw Exception('Failed to fetch VIPs: $e');
    }
  }
}