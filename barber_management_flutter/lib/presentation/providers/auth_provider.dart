import 'package:flutter/material.dart';
import 'package:barbermanagemobile/data/models/auth/user_model.dart';
import 'package:barbermanagemobile/domain/usecases/login_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/register_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/get_user_by_id_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/update_avatar_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/update_user_info_use_case.dart'; // New import
import 'package:get_it/get_it.dart';
import 'dart:io';

class AuthProvider with ChangeNotifier {
  final LoginUseCase loginUseCase;
  final RegisterUseCase registerUseCase;
  final GetUserByIDUseCase _getUserByIDUseCase = GetIt.instance<GetUserByIDUseCase>();
  final UpdateAvatarUseCase _updateAvatarUseCase = GetIt.instance<UpdateAvatarUseCase>();
  final UpdateUserInfoUseCase _updateUserInfoUseCase = GetIt.instance<UpdateUserInfoUseCase>(); // New UseCase

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
        print('AuthProvider: Login failed: $failure');
      },
      (success) {
        _user = success;
        _isLoading = false;
        notifyListeners();
        print('AuthProvider: Login successful: ${success.toJson()}');
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
        print('AuthProvider: Registration failed: $failure');
      },
      (success) {
        _user = success;
        _isLoading = false;
        notifyListeners();
        print('AuthProvider: Registration successful: ${success.toJson()}');
      },
    );
  }

  Future<void> updateAvatar(File image) async {
    if (_user?.userId == null || int.tryParse(_user!.userId) == null) {
      _errorMessage = 'Không tìm thấy thông tin người dùng';
      notifyListeners();
      print('AuthProvider: Invalid userId for avatar update');
      return;
    }

    _isLoading = true;
    _errorMessage = null;
    notifyListeners();

    final userId = int.parse(_user!.userId);
    final result = await _updateAvatarUseCase.call(userId, image);
    result.fold(
      (failure) {
        _errorMessage = failure;
        _isLoading = false;
        notifyListeners();
        print('AuthProvider: Avatar update failed: $failure');
      },
      (_) async {
        final userResult = await _getUserByIDUseCase.call(userId);
        userResult.fold(
          (failure) {
            _errorMessage = 'Cập nhật ảnh thành công nhưng không thể làm mới dữ liệu: $failure';
            _isLoading = false;
            notifyListeners();
            print('AuthProvider: Failed to refresh user data: $failure');
          },
          (updatedUser) {
            _user = updatedUser;
            _isLoading = false;
            notifyListeners();
            print('AuthProvider: Avatar updated and user refreshed: ${updatedUser.toJson()}');
          },
        );
      },
    );
  }

  Future<void> updateUserInfo({
    required int userID,
    required String fullName,
    String? userName,
    String? userEmail,
    String? userPhone,
    String? dateOfBirth,
    String? gender,
  }) async {
    if (_user?.userId == null || int.tryParse(_user!.userId) != userID) {
      _errorMessage = 'Thông tin người dùng không khớp';
      notifyListeners();
      print('AuthProvider: User ID mismatch for update');
      return;
    }

    _isLoading = true;
    _errorMessage = null;
    notifyListeners();

    final result = await _updateUserInfoUseCase.call(
      userID: userID,
      fullName: fullName,
      userName: userName,
      userEmail: userEmail,
      userPhone: userPhone,
      dateOfBirth: dateOfBirth,
      gender: gender,
    );
    result.fold(
      (failure) {
        _errorMessage = failure;
        _isLoading = false;
        notifyListeners();
        print('AuthProvider: User info update failed: $failure');
      },
      (_) async {
        final userResult = await _getUserByIDUseCase.call(userID);
        userResult.fold(
          (failure) {
            _errorMessage = 'Cập nhật thông tin thành công nhưng không thể làm mới dữ liệu: $failure';
            _isLoading = false;
            notifyListeners();
            print('AuthProvider: Failed to refresh user data: $failure');
          },
          (updatedUser) {
            _user = updatedUser;
            _isLoading = false;
            notifyListeners();
            print('AuthProvider: User info updated and refreshed: ${updatedUser.toJson()}');
          },
        );
      },
    );
  }

  void updateUser(UserModel user) {
    _user = user;
    _errorMessage = null;
    _isLoading = false;
    notifyListeners();
    print('AuthProvider: User updated: ${user.toJson()}');
  }

  Future<void> logout() async {
    _user = null;
    _errorMessage = null;
    notifyListeners();
    print('AuthProvider: Logged out');
  }
}