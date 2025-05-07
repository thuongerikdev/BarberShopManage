import 'package:flutter/material.dart';
import 'package:barbermanagemobile/data/models/user_model.dart';
import 'package:barbermanagemobile/domain/usecases/login_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/register_use_case.dart';

class AuthProvider with ChangeNotifier {
  final LoginUseCase loginUseCase;
  final RegisterUseCase registerUseCase;

  UserModel? _user;
  bool _isLoading = false;
  String? _errorMessage;

  AuthProvider(this.loginUseCase, this.registerUseCase);

  UserModel? get user => _user;
  bool get isLoading => _isLoading;
  String? get errorMessage => _errorMessage;

  Future<void> login(String email, String password) async {
    _isLoading = true;
    _errorMessage = null;
    notifyListeners();

    final result = await loginUseCase(email, password);
    result.fold(
      (failure) {
        _errorMessage = failure;
        _isLoading = false;
        notifyListeners();
      },
      (success) {
        _user = success;
        _isLoading = false;
        notifyListeners();
      },
    );
  }

  Future<void> register({
    required int roleID,
    required String userName,
    required String password,
    required String email,
    required String phoneNumber,
    required String fullName,
    required String dateOfBirth,
    required String gender,
  }) async {
    _isLoading = true;
    _errorMessage = null;
    notifyListeners();

    final result = await registerUseCase(
      roleID: roleID,
      userName: userName,
      password: password,
      email: email,
      phoneNumber: phoneNumber,
      fullName: fullName,
      dateOfBirth: dateOfBirth,
      gender: gender,
    );
    result.fold(
      (failure) {
        _errorMessage = failure;
        _isLoading = false;
        notifyListeners();
      },
      (success) {
        _user = success;
        _isLoading = false;
        notifyListeners();
      },
    );
  }

  void logout() {
    _user = null;
    _errorMessage = null;
    notifyListeners();
  }
}