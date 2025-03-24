import 'package:flutter/material.dart';
import '../../domain/entities/user.dart';
import '../../domain/usecases/login_use_case.dart';

class AuthProvider with ChangeNotifier {
  final LoginUseCase loginUseCase;
  bool isLoading = false;
  String? errorMessage;
  User? user;

  AuthProvider(this.loginUseCase);

  Future<void> login(String email, String password) async {
    isLoading = true;
    errorMessage = null;
    notifyListeners();

    final result = await loginUseCase(email, password);
    result.fold(
      (failure) => errorMessage = failure,
      (success) => user = success,
    );

    isLoading = false;
    notifyListeners();
  }

  void logout() {
    user = null;
    notifyListeners();
  }
}