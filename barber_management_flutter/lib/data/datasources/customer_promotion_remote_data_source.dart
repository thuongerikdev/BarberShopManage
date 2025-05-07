import 'dart:convert';
import 'package:http/http.dart' as http;
import 'package:flutter_dotenv/flutter_dotenv.dart';
import '../models/customer_promotion_model.dart';

abstract class CustomerPromotionRemoteDataSource {
  Future<List<CustomerPromotionModel>> getCustomerPromotions(int customerId);
  Future<void> createCustomerPromotion(int customerId, int promoId, String status);
}

class CustomerPromotionRemoteDataSourceImpl implements CustomerPromotionRemoteDataSource {
  final String baseUrl = dotenv.get('BASE_URL');

  @override
  Future<List<CustomerPromotionModel>> getCustomerPromotions(int customerId) async {
    final response = await http.get(
      Uri.parse('$baseUrl/AuthCusPromo/getcuspromobycustomerid/$customerId'),
      headers: {'accept': '*/*'},
    );

    if (response.statusCode == 200) {
      final jsonData = jsonDecode(response.body);
      if (jsonData['errorCode'] == 200) {
        return List<Map<String, dynamic>>.from(jsonData['data'])
            .map((json) => CustomerPromotionModel.fromJson(json))
            .toList();
      } else {
        throw Exception(jsonData['errorMessager'] ?? 'Lấy khuyến mãi khách hàng thất bại');
      }
    } else {
      throw Exception('Lỗi server: ${response.statusCode}');
    }
  }

  @override
  Future<void> createCustomerPromotion(int customerId, int promoId, String status) async {
    final response = await http.post(
      Uri.parse('$baseUrl/AuthCusPromo/createcuspromo'),
      headers: {
        'accept': '*/*',
        'Content-Type': 'application/json',
      },
      body: jsonEncode({
        'customerID': customerId,
        'promoID': promoId,
        'cusPromoStatus': status,
      }),
    );

    if (response.statusCode == 200) {
      final jsonData = jsonDecode(response.body);
      if (jsonData['errorCode'] != 200) {
        throw Exception(jsonData['errorMessager'] ?? 'Tạo khuyến mãi khách hàng thất bại');
      }
    } else {
      throw Exception('Lỗi server: ${response.statusCode}');
    }
  }
}