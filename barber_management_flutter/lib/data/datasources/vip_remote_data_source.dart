import 'dart:convert';
import 'package:http/http.dart' as http;
import 'package:flutter_dotenv/flutter_dotenv.dart';
import '../models/vip_model.dart';

abstract class VipRemoteDataSource {
  Future<List<VipModel>> getVips();
}

class VipRemoteDataSourceImpl implements VipRemoteDataSource {
  final String baseUrl = dotenv.get('BASE_URL');

  @override
  Future<List<VipModel>> getVips() async {
    final response = await http.get(
      Uri.parse('$baseUrl/AuthVip/getallvip'),
      headers: {'accept': '*/*'},
    );

    if (response.statusCode == 200) {
      final jsonData = jsonDecode(response.body);
      if (jsonData['errorCode'] == 200) {
        return List<Map<String, dynamic>>.from(jsonData['data'])
            .map((json) => VipModel.fromJson(json))
            .toList();
      } else {
        throw Exception(jsonData['errorMessager'] ?? 'Lấy danh sách VIP thất bại');
      }
    } else {
      throw Exception('Lỗi server: ${response.statusCode}');
    }
  }
}