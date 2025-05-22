import 'dart:convert';
import 'package:http/http.dart' as http;
import 'package:flutter_dotenv/flutter_dotenv.dart';

abstract class BookingCategoryRemoteDataSource {
  Future<List<Map<String, dynamic>>> getBookingCategories();
}

class BookingCategoryRemoteDataSourceImpl implements BookingCategoryRemoteDataSource {
  final String baseUrl = dotenv.get('BASE_URL');

  @override
  Future<List<Map<String, dynamic>>> getBookingCategories() async {
    final url = Uri.parse('$baseUrl/BookingCategory/getall');
    final headers = {'accept': '*/*'};

    final response = await http.get(
      url,
      headers: headers,
    );

    if (response.statusCode == 200) {
      final jsonData = jsonDecode(response.body);
      if (jsonData['errorCode'] == 200) {
        final List<dynamic> data = jsonData['data'];
        return data.map((item) {
          return {
            'categoryID': item['categoryID']?.toString() ?? '',
            'categoryName': item['categoryName']?.toString() ?? 'Unknown',
            'categoryDescription': item['categoryDescription']?.toString() ?? '',
            'categoryPrice': item['categoryPrice']?.toString() ?? '0',
            'categoryImage': item['categoryImage']?.toString() ?? '',
          };
        }).toList();
      } else {
        throw Exception(jsonData['errorMessager'] ?? 'Lấy danh sách category thất bại');
      }
    } else {
      throw Exception('Lỗi server: ${response.statusCode}');
    }
  }
}