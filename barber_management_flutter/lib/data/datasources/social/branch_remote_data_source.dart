import 'dart:convert';
import 'package:http/http.dart' as http;
import 'package:flutter_dotenv/flutter_dotenv.dart';
import '../../models/social/branch_model.dart';

abstract class BranchRemoteDataSource {
  Future<List<BranchModel>> getBranches();
}

class BranchRemoteDataSourceImpl implements BranchRemoteDataSource {
  final String baseUrl = dotenv.get('BASE_URL');

  @override
  Future<List<BranchModel>> getBranches() async {
    final response = await http.get(
      Uri.parse('$baseUrl/AuthBranch/getallbranch'),
      headers: {'accept': '*/*'},
    );

    if (response.statusCode == 200) {
      final jsonData = jsonDecode(response.body);
      if (jsonData['errorCode'] == 200) {
        return List<Map<String, dynamic>>.from(jsonData['data'])
            .map((json) => BranchModel.fromJson(json))
            .toList();
      } else {
        throw Exception(jsonData['errorMessager'] ?? 'Lấy danh sách chi nhánh thất bại');
      }
    } else {
      throw Exception('Lỗi server: ${response.statusCode}');
    }
  }
}