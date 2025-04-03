// lib/data/datasources/slider_remote_data_source.dart
import 'dart:convert';
import 'package:http/http.dart' as http;
import 'package:flutter_dotenv/flutter_dotenv.dart';

class SliderRemoteDataSource {
  final String baseUrl = dotenv.get('BASE_URL'); // Lấy từ .env

  Future<List<Map<String, String>>> getSliderImages() async {
    final url = Uri.parse('$baseUrl/SocialSrc/getsrcbyname/slider');
    final response = await http.get(url);

    if (response.statusCode == 200) {
      final jsonData = jsonDecode(response.body);
      if (jsonData['errorCode'] == 0) {
        final List<dynamic> data = jsonData['data'];
        return data.map((item) => {
          'image': item['imageSrc'] as String,
          'title': item['srcName'] as String,
        }).toList();
      } else {
        throw Exception(jsonData['errorMessager'] ?? 'Lấy danh sách thất bại');
      }
    } else {
      throw Exception('Lỗi server: ${response.statusCode}');
    }
  }
}