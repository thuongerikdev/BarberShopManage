import 'dart:convert';
import 'package:http/http.dart' as http;
import 'package:flutter_dotenv/flutter_dotenv.dart';
import 'package:flutter/foundation.dart';
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

    if (kDebugMode) {
      print('API Response for customer $customerId: ${response.body}');
    }

    if (response.statusCode == 200) {
      final jsonData = jsonDecode(response.body);
      if (jsonData['errorCode'] == 200) {
        final dataList = jsonData['data'] as List<dynamic>? ?? [];
        return dataList.map((json) => CustomerPromotionModel.fromJson(json as Map<String, dynamic>)).toList();
      } else if (jsonData['errorCode'] == 404 || jsonData['errorMessager']?.toString().contains('Không tìm thấy khuyến mãi') == true) {
        return []; // Return empty list if no promotions are found
      } else {
        throw Exception(jsonData['errorMessager'] ?? 'Lấy khuyến mãi khách hàng thất bại');
      }
    } else {
      throw Exception('Lỗi server: ${response.statusCode} - ${response.body}');
    }
  }

  @override
  Future<void> createCustomerPromotion(int customerId, int promoId, String status) async {
    final response = await http.post(
      Uri.parse('$baseUrl/AuthCusPromo/customergetpromo'),
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

    if (kDebugMode) {
      print('API Response for creating promotion: ${response.body}');
    }

    if (response.statusCode == 200) {
      final jsonData = jsonDecode(response.body);
      if (jsonData['errorCode'] != 200) {
        throw Exception(jsonData['errorMessager'] ?? 'Tạo khuyến mãi khách hàng thất bại');
      }
    } else {
      throw Exception('Lỗi server: ${response.statusCode} - ${response.body}');
    }
  }
}