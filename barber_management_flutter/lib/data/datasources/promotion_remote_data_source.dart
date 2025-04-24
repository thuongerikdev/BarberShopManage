import 'dart:convert';
import 'package:http/http.dart' as http;
import 'package:flutter_dotenv/flutter_dotenv.dart';
import '../models/promotion_model.dart';

abstract class PromotionRemoteDataSource {
  Future<List<PromotionModel>> getPromotions();
}

class PromotionRemoteDataSourceImpl implements PromotionRemoteDataSource {
  final String baseUrl = dotenv.get('BASE_URL');

  @override
  Future<List<PromotionModel>> getPromotions() async {
    final response = await http.get(
      Uri.parse('$baseUrl/AuthPromotion/getall'),
      headers: {'accept': '*/*'},
    );

    if (response.statusCode == 200) {
      final jsonData = jsonDecode(response.body);
      if (jsonData['errorCode'] == 200) {
        return List<Map<String, dynamic>>.from(jsonData['data'])
            .map((json) => PromotionModel.fromJson(json))
            .toList();
      } else {
        throw Exception(jsonData['errorMessager'] ?? 'Lấy khuyến mãi thất bại');
      }
    } else {
      throw Exception('Lỗi server: ${response.statusCode}');
    }
  }
}