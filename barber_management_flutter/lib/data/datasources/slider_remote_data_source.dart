// lib/data/datasources/slider_remote_data_source.dart
import 'dart:convert';
import 'package:http/http.dart' as http;
import 'package:flutter_dotenv/flutter_dotenv.dart';

abstract class SliderRemoteDataSource {
  Future<List<Map<String, String>>> getSliderImages();
  Future<Map<String, String>> getCommitmentImage(); // New method
}

class SliderRemoteDataSourceImpl implements SliderRemoteDataSource {
  final String baseUrl = dotenv.get('BASE_URL');

  @override
  Future<List<Map<String, String>>> getSliderImages() async {
    final url = Uri.parse('$baseUrl/SocialSrc/getsrcbyname/slider');
    final response = await http.get(url, headers: {'accept': '*/*'});

    if (response.statusCode == 200) {
      final jsonData = jsonDecode(response.body);
      if (jsonData['errorCode'] == 200) {
        final List<dynamic> data = jsonData['data'];
        return data.map((item) => {
              'image': item['imageSrc']?.toString() ?? '',
              'title': item['srcName']?.toString() ?? '',
            }).toList();
      } else {
        throw Exception(jsonData['errorMessager'] ?? 'Lấy danh sách slider thất bại');
      }
    } else {
      throw Exception('Lỗi server: ${response.statusCode}');
    }
  }

  @override
  Future<Map<String, String>> getCommitmentImage() async {
    final url = Uri.parse('$baseUrl/SocialSrc/getsrcbyname/Commitment');
    final response = await http.get(url, headers: {'accept': '*/*'});

    if (response.statusCode == 200) {
      final jsonData = jsonDecode(response.body);
      if (jsonData['errorCode'] == 200) {
        final List<dynamic> data = jsonData['data'];
        if (data.isNotEmpty) {
          return {
            'image': data[0]['imageSrc']?.toString() ?? '',
            'title': data[0]['srcName']?.toString() ?? '',
          };
        } else {
          throw Exception('Không tìm thấy hình ảnh Commitment');
        }
      } else {
        throw Exception(jsonData['errorMessager'] ?? 'Lấy hình ảnh Commitment thất bại');
      }
    } else {
      throw Exception('Lỗi server: ${response.statusCode}');
    }
  }
}