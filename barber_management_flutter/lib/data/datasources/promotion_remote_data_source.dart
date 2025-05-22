import 'dart:convert';
import 'package:http/http.dart' as http;
import 'package:flutter_dotenv/flutter_dotenv.dart';
import 'package:flutter/foundation.dart';
import '../models/promotion_model.dart';

abstract class PromotionRemoteDataSource {
  Future<List<PromotionModel>> getAllPromoByCustomer(int customerId);
  Future<void> createCustomerPromotion(int customerId, int promoId, String status);
}

class PromotionRemoteDataSourceImpl implements PromotionRemoteDataSource {
  final String baseUrl = dotenv.get('BASE_URL');

  @override
  Future<List<PromotionModel>> getAllPromoByCustomer(int customerId) async {
    final response = await http.get(
      Uri.parse('$baseUrl/AuthPromotion/GetAllPromoByCustomer/$customerId'),
      headers: {'accept': '*/*'},
    );

    if (kDebugMode) {
      print('API Response for GetAllPromoByCustomer/$customerId: ${response.body}');
    }

    if (response.statusCode == 200) {
      final jsonData = jsonDecode(response.body);
      if (jsonData['errorCode'] == 200) {
        final dataList = jsonData['data'] as List<dynamic>? ?? [];
        return dataList.map((json) => PromotionModel.fromJson(json as Map<String, dynamic>)).toList();
      } else {
        throw Exception(jsonData['errorMessager'] ?? 'Lấy danh sách khuyến mãi thất bại');
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