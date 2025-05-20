import 'dart:convert';
import 'package:http/http.dart' as http;
import 'package:flutter_dotenv/flutter_dotenv.dart';
import 'package:barbermanagemobile/data/models/booking_product_model.dart';

abstract class BookingProductRemoteDataSource {
  Future<List<BookingProductModel>> getBookingProducts();
  Future<BookingProductModel> getBookingProductById(int productId);
  Future<List<BookingProductModel>> getBookingProductsByCategory(int categoryId);
}

class BookingProductRemoteDataSourceImpl implements BookingProductRemoteDataSource {
  final String baseUrl = dotenv.get('BASE_URL');

  @override
  Future<List<BookingProductModel>> getBookingProducts() async {
    final url = Uri.parse('$baseUrl/BookingProduct/getall');
    final headers = {'accept': '*/*'};

    final response = await http.get(
      url,
      headers: headers,
    );

    if (response.statusCode == 200) {
      final jsonData = jsonDecode(response.body);
      if (jsonData['errorCode'] == 200) {
        final List<dynamic> data = jsonData['data'];
        return data.map((item) => BookingProductModel.fromJson(item)).toList();
      } else {
        throw Exception(jsonData['errorMessager'] ?? 'Lấy danh sách sản phẩm thất bại');
      }
    } else {
      throw Exception('Lỗi server: ${response.statusCode}');
    }
  }

  @override
  Future<BookingProductModel> getBookingProductById(int productId) async {
    final url = Uri.parse('$baseUrl/BookingProduct/get/$productId');
    final headers = {'accept': '*/*'};

    final response = await http.get(
      url,
      headers: headers,
    );

    if (response.statusCode == 200) {
      final jsonData = jsonDecode(response.body);
      if (jsonData['errorCode'] == 200) {
        final Map<String, dynamic> data = jsonData['data'];
        return BookingProductModel.fromJson(data);
      } else {
        throw Exception(jsonData['errorMessager'] ?? 'Lấy thông tin sản phẩm thất bại');
      }
    } else {
      throw Exception('Lỗi server: ${response.statusCode}');
    }
  }

  @override
  Future<List<BookingProductModel>> getBookingProductsByCategory(int categoryId) async {
    final url = Uri.parse('$baseUrl/BookingProduct/getbycategory/$categoryId');
    final headers = {'accept': '*/*'};

    final response = await http.get(
      url,
      headers: headers,
    );

    if (response.statusCode == 200) {
      final jsonData = jsonDecode(response.body);
      if (jsonData['errorCode'] == 200) {
        final List<dynamic> data = jsonData['data'];
        return data.map((item) => BookingProductModel.fromJson(item)).toList();
      } else {
        throw Exception(jsonData['errorMessager'] ?? 'Lấy danh sách sản phẩm theo danh mục thất bại');
      }
    } else {
      throw Exception('Lỗi server: ${response.statusCode}');
    }
  }
}