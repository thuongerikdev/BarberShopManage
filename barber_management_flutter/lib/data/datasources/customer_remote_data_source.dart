import 'dart:convert';
import 'package:http/http.dart' as http;
import 'package:flutter_dotenv/flutter_dotenv.dart';
import 'package:barbermanagemobile/data/models/customer_model.dart';

abstract class CustomerRemoteDataSource {
  Future<CustomerModel> getCustomerByUserID(int userID);
}

class CustomerRemoteDataSourceImpl implements CustomerRemoteDataSource {
  final String baseUrl = dotenv.get('BASE_URL');

  @override
  Future<CustomerModel> getCustomerByUserID(int userID) async {
    final response = await http.get(
      Uri.parse('$baseUrl/AuthCustomer/getByUserID/$userID'),
      headers: {'accept': '*/*'},
    );

    if (response.statusCode == 200) {
      final jsonResponse = jsonDecode(response.body);
      if (jsonResponse['errorCode'] == 200) {
        return CustomerModel.fromJson(jsonResponse['data']);
      } else {
        throw Exception(jsonResponse['errorMessager'] ?? 'Lấy thông tin khách hàng thất bại');
      }
    } else {
      throw Exception('Lỗi server: ${response.statusCode}');
    }
  }
}