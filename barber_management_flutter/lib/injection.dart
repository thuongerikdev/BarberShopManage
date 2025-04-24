import 'package:get_it/get_it.dart';
import 'package:barbermanagemobile/data/datasources/auth_remote_data_source.dart';
import 'package:barbermanagemobile/data/datasources/booking_order_remote_data_source.dart';
import 'package:barbermanagemobile/data/datasources/booking_service_remote_data_source.dart';
import 'package:barbermanagemobile/data/datasources/branch_remote_data_source.dart';
import 'package:barbermanagemobile/data/datasources/customer_remote_data_source.dart';
import 'package:barbermanagemobile/data/datasources/employee_remote_data_source.dart';
import 'package:barbermanagemobile/data/datasources/promotion_remote_data_source.dart';
import 'package:barbermanagemobile/data/datasources/slider_remote_data_source.dart';
import 'package:barbermanagemobile/data/datasources/booking_category_remote_data_source.dart';
import 'package:barbermanagemobile/data/datasources/vip_remote_data_source.dart';
import 'package:barbermanagemobile/data/repositories/auth_repository_impl.dart';
import 'package:barbermanagemobile/data/repositories/booking_order_repository_impl.dart';
import 'package:barbermanagemobile/data/repositories/booking_service_repository_impl.dart';
import 'package:barbermanagemobile/data/repositories/branch_repository_impl.dart';
import 'package:barbermanagemobile/data/repositories/customer_repository_impl.dart';
import 'package:barbermanagemobile/data/repositories/employee_repository_impl.dart';
import 'package:barbermanagemobile/data/repositories/promotion_repository_impl.dart';
import 'package:barbermanagemobile/data/repositories/slider_repository_impl.dart';
import 'package:barbermanagemobile/data/repositories/booking_category_repository_impl.dart';
import 'package:barbermanagemobile/data/repositories/vip_repository_impl.dart';
import 'package:barbermanagemobile/domain/repositories/auth_repository.dart';
import 'package:barbermanagemobile/domain/repositories/booking_order_repository.dart';
import 'package:barbermanagemobile/domain/repositories/booking_service_repository.dart';
import 'package:barbermanagemobile/domain/repositories/branch_repository.dart';
import 'package:barbermanagemobile/domain/repositories/customer_repository.dart';
import 'package:barbermanagemobile/domain/repositories/employee_repository.dart';
import 'package:barbermanagemobile/domain/repositories/promotion_repository.dart';
import 'package:barbermanagemobile/domain/repositories/slider_repository.dart';
import 'package:barbermanagemobile/domain/repositories/booking_category_repository.dart';
import 'package:barbermanagemobile/domain/repositories/vip_repository.dart';
import 'package:barbermanagemobile/domain/usecases/create_booking_order_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/get_all_branches_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/get_booking_service_detail_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/get_booking_services_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/get_customer_by_user_id_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/get_employees_by_branch_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/get_employees_by_date_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/get_employees_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/get_orders_by_customer_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/get_promotions_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/get_slider_images_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/get_booking_categories_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/get_vips_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/get_branches_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/get_user_by_id_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/login_use_case.dart';
import 'package:barbermanagemobile/presentation/providers/auth_provider.dart';
import 'package:barbermanagemobile/domain/usecases/delete_booking_order_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/pay_booking_order_use_case.dart';

final GetIt getIt = GetIt.instance;

// Register dependencies for Auth module
void _registerAuthDependencies(GetIt sl) {
  sl.registerLazySingleton<AuthRemoteDataSource>(
    () => AuthRemoteDataSourceImpl(),
  );
  sl.registerLazySingleton<AuthRepository>(
    () => AuthRepositoryImpl(sl<AuthRemoteDataSource>()),
  );
  sl.registerLazySingleton<LoginUseCase>(
    () => LoginUseCase(sl<AuthRepository>()),
  );
  sl.registerLazySingleton<GetUserByIDUseCase>(
    () => GetUserByIDUseCase(sl<AuthRepository>()),
  );
  sl.registerFactory<AuthProvider>(
    () => AuthProvider(sl<LoginUseCase>()),
  );
}

// Register dependencies for Customer module
void _registerCustomerDependencies(GetIt sl) {
  sl.registerLazySingleton<CustomerRemoteDataSource>(
    () => CustomerRemoteDataSourceImpl(),
  );
  sl.registerLazySingleton<CustomerRepository>(
    () => CustomerRepositoryImpl(sl<CustomerRemoteDataSource>()),
  );
  sl.registerLazySingleton<GetCustomerByUserIDUseCase>(
    () => GetCustomerByUserIDUseCase(sl<CustomerRepository>()),
  );
}

// Register dependencies for Booking module
void _registerBookingDependencies(GetIt sl) {
  sl.registerLazySingleton<BookingServiceRemoteDataSource>(
    () => BookingServiceRemoteDataSourceImpl(),
  );
  sl.registerLazySingleton<BookingServiceRepository>(
    () => BookingServiceRepositoryImpl(sl<BookingServiceRemoteDataSource>()),
  );
  sl.registerLazySingleton<BookingOrderRemoteDataSource>(
    () => BookingOrderRemoteDataSourceImpl(),
  );
  sl.registerLazySingleton<BookingOrderRepository>(
    () => BookingOrderRepositoryImpl(sl<BookingOrderRemoteDataSource>()),
  );
  sl.registerLazySingleton<GetBookingServicesUseCase>(
    () => GetBookingServicesUseCase(sl<BookingServiceRepository>()),
  );
  sl.registerLazySingleton<GetBookingServiceDetailUseCase>(
    () => GetBookingServiceDetailUseCase(sl<BookingServiceRepository>()),
  );
  sl.registerLazySingleton<CreateBookingOrderUseCase>(
    () => CreateBookingOrderUseCase(sl<BookingOrderRepository>()),
  );
  sl.registerLazySingleton<GetAllBranchesUseCase>(
    () => GetAllBranchesUseCase(sl<BookingServiceRepository>()),
  );
  sl.registerLazySingleton<GetEmployeesByBranchUseCase>(
    () => GetEmployeesByBranchUseCase(sl<BookingServiceRepository>()),
  );
  sl.registerLazySingleton<GetEmployeesByDateUseCase>(
    () => GetEmployeesByDateUseCase(sl<BookingServiceRepository>()),
  );
  sl.registerLazySingleton<GetOrdersByCustomerUseCase>(
    () => GetOrdersByCustomerUseCase(sl<BookingOrderRepository>()),
  );
  sl.registerLazySingleton<DeleteBookingOrderUseCase>(
    () => DeleteBookingOrderUseCase(sl<BookingOrderRepository>()),
  );
  sl.registerLazySingleton<PayBookingOrderUseCase>(
  () => PayBookingOrderUseCase(sl<BookingOrderRepository>()),
);
}

// Register dependencies for Employee module
void _registerEmployeeDependencies(GetIt sl) {
  sl.registerLazySingleton<EmployeeRemoteDataSource>(
    () => EmployeeRemoteDataSourceImpl(),
  );
  sl.registerLazySingleton<EmployeeRepository>(
    () => EmployeeRepositoryImpl(sl<EmployeeRemoteDataSource>()),
  );
  sl.registerLazySingleton<GetEmployeesUseCase>(
    () => GetEmployeesUseCase(sl<EmployeeRepository>()),
  );
}

// Register dependencies for Slider module
void _registerSliderDependencies(GetIt sl) {
  sl.registerLazySingleton<SliderRemoteDataSource>(
    () => SliderRemoteDataSourceImpl(),
  );
  sl.registerLazySingleton<SliderRepository>(
    () => SliderRepositoryImpl(sl<SliderRemoteDataSource>()),
  );
  sl.registerLazySingleton<GetSliderImagesUseCase>(
    () => GetSliderImagesUseCase(sl<SliderRepository>()),
  );
}

// Register dependencies for Booking Category module
void _registerBookingCategoryDependencies(GetIt sl) {
  sl.registerLazySingleton<BookingCategoryRemoteDataSource>(
    () => BookingCategoryRemoteDataSourceImpl(),
  );
  sl.registerLazySingleton<BookingCategoryRepository>(
    () => BookingCategoryRepositoryImpl(sl<BookingCategoryRemoteDataSource>()),
  );
  sl.registerLazySingleton<GetBookingCategoriesUseCase>(
    () => GetBookingCategoriesUseCase(sl<BookingCategoryRepository>()),
  );
}

// Register dependencies for Promotion module
void _registerPromotionDependencies(GetIt sl) {
  sl.registerLazySingleton<PromotionRemoteDataSource>(
    () => PromotionRemoteDataSourceImpl(),
  );
  sl.registerLazySingleton<PromotionRepository>(
    () => PromotionRepositoryImpl(sl<PromotionRemoteDataSource>()),
  );
  sl.registerLazySingleton<GetPromotionsUseCase>(
    () => GetPromotionsUseCase(sl<PromotionRepository>()),
  );
}

// Register dependencies for VIP module
void _registerVipDependencies(GetIt sl) {
  sl.registerLazySingleton<VipRemoteDataSource>(
    () => VipRemoteDataSourceImpl(),
  );
  sl.registerLazySingleton<VipRepository>(
    () => VipRepositoryImpl(sl<VipRemoteDataSource>()),
  );
  sl.registerLazySingleton<GetVipsUseCase>(
    () => GetVipsUseCase(sl<VipRepository>()),
  );
}

// Register dependencies for Branch module
void _registerBranchDependencies(GetIt sl) {
  sl.registerLazySingleton<BranchRemoteDataSource>(
    () => BranchRemoteDataSourceImpl(),
  );
  sl.registerLazySingleton<BranchRepository>(
    () => BranchRepositoryImpl(sl<BranchRemoteDataSource>()),
  );
  sl.registerLazySingleton<GetBranchesUseCase>(
    () => GetBranchesUseCase(sl<BranchRepository>()),
  );
}

// Setup all dependencies
void setupDependencies() {
  final sl = getIt;
  _registerAuthDependencies(sl);
  _registerCustomerDependencies(sl);
  _registerBookingDependencies(sl);
  _registerEmployeeDependencies(sl);
  _registerSliderDependencies(sl);
  _registerBookingCategoryDependencies(sl);
  _registerPromotionDependencies(sl);
  _registerVipDependencies(sl);
  _registerBranchDependencies(sl);
}

// Verify that critical dependencies are registered
void verifyDependencies() {
  if (!getIt.isRegistered<AuthRepository>()) {
    throw Exception('AuthRepository is not registered');
  }
  if (!getIt.isRegistered<CustomerRepository>()) {
    throw Exception('CustomerRepository is not registered');
  }
  if (!getIt.isRegistered<BookingServiceRepository>()) {
    throw Exception('BookingServiceRepository is not registered');
  }
  if (!getIt.isRegistered<BookingOrderRepository>()) {
    throw Exception('BookingOrderRepository is not registered');
  }
  if (!getIt.isRegistered<EmployeeRepository>()) {
    throw Exception('EmployeeRepository is not registered');
  }
  if (!getIt.isRegistered<SliderRepository>()) {
    throw Exception('SliderRepository is not registered');
  }
  if (!getIt.isRegistered<BookingCategoryRepository>()) {
    throw Exception('BookingCategoryRepository is not registered');
  }
  if (!getIt.isRegistered<PromotionRepository>()) {
    throw Exception('PromotionRepository is not registered');
  }
  if (!getIt.isRegistered<VipRepository>()) {
    throw Exception('VipRepository is not registered');
  }
  if (!getIt.isRegistered<BranchRepository>()) {
    throw Exception('BranchRepository is not registered');
  }
  if (!getIt.isRegistered<LoginUseCase>()) {
    throw Exception('LoginUseCase is not registered');
  }
  if (!getIt.isRegistered<GetUserByIDUseCase>()) {
    throw Exception('GetUserByIDUseCase is not registered');
  }
  if (!getIt.isRegistered<GetCustomerByUserIDUseCase>()) {
    throw Exception('GetCustomerByUserIDUseCase is not registered');
  }
  if (!getIt.isRegistered<GetBookingServiceDetailUseCase>()) {
    throw Exception('GetBookingServiceDetailUseCase is not registered');
  }
  if (!getIt.isRegistered<CreateBookingOrderUseCase>()) {
    throw Exception('CreateBookingOrderUseCase is not registered');
  }
  if (!getIt.isRegistered<GetAllBranchesUseCase>()) {
    throw Exception('GetAllBranchesUseCase is not registered');
  }
  if (!getIt.isRegistered<GetEmployeesByBranchUseCase>()) {
    throw Exception('GetEmployeesByBranchUseCase is not registered');
  }
  if (!getIt.isRegistered<GetEmployeesByDateUseCase>()) {
    throw Exception('GetEmployeesByDateUseCase is not registered');
  }
  if (!getIt.isRegistered<GetOrdersByCustomerUseCase>()) {
    throw Exception('GetOrdersByCustomerUseCase is not registered');
  }
  if (!getIt.isRegistered<GetPromotionsUseCase>()) {
    throw Exception('GetPromotionsUseCase is not registered');
  }
  if (!getIt.isRegistered<GetVipsUseCase>()) {
    throw Exception('GetVipsUseCase is not registered');
  }
  if (!getIt.isRegistered<GetBranchesUseCase>()) {
    throw Exception('GetBranchesUseCase is not registered');
  }
  if (!getIt.isRegistered<DeleteBookingOrderUseCase>()) {
    throw Exception('DeleteBookingOrderUseCase is not registered');
  }
  if (!getIt.isRegistered<PayBookingOrderUseCase>()) {
  throw Exception('PayBookingOrderUseCase is not registered');
}
}