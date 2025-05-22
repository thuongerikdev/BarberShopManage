import 'package:barbermanagemobile/domain/usecases/get_commitment_image_use_case.dart';
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
import 'package:barbermanagemobile/data/datasources/booking_product_remote_data_source.dart';
import 'package:barbermanagemobile/data/datasources/vip_remote_data_source.dart';
import 'package:barbermanagemobile/data/datasources/blog_remote_data_source.dart';
import 'package:barbermanagemobile/data/datasources/check_in_remote_data_source.dart';
import 'package:barbermanagemobile/data/repositories/auth_repository_impl.dart';
import 'package:barbermanagemobile/data/repositories/booking_order_repository_impl.dart';
import 'package:barbermanagemobile/data/repositories/booking_service_repository_impl.dart';
import 'package:barbermanagemobile/data/repositories/branch_repository_impl.dart';
import 'package:barbermanagemobile/data/repositories/customer_repository_impl.dart';
import 'package:barbermanagemobile/data/repositories/employee_repository_impl.dart';
import 'package:barbermanagemobile/data/repositories/promotion_repository_impl.dart';
import 'package:barbermanagemobile/data/repositories/slider_repository_impl.dart';
import 'package:barbermanagemobile/data/repositories/booking_category_repository_impl.dart';
import 'package:barbermanagemobile/data/repositories/booking_product_repository_impl.dart';
import 'package:barbermanagemobile/data/repositories/vip_repository_impl.dart';
import 'package:barbermanagemobile/data/repositories/blog_repository_impl.dart';
import 'package:barbermanagemobile/data/repositories/check_in_repository_impl.dart';
import 'package:barbermanagemobile/domain/repositories/auth_repository.dart';
import 'package:barbermanagemobile/domain/repositories/booking_order_repository.dart';
import 'package:barbermanagemobile/domain/repositories/booking_service_repository.dart';
import 'package:barbermanagemobile/domain/repositories/branch_repository.dart';
import 'package:barbermanagemobile/domain/repositories/customer_repository.dart';
import 'package:barbermanagemobile/domain/repositories/employee_repository.dart';
import 'package:barbermanagemobile/domain/repositories/promotion_repository.dart';
import 'package:barbermanagemobile/domain/repositories/slider_repository.dart';
import 'package:barbermanagemobile/domain/repositories/booking_category_repository.dart';
import 'package:barbermanagemobile/domain/repositories/booking_product_repository.dart';
import 'package:barbermanagemobile/domain/repositories/vip_repository.dart';
import 'package:barbermanagemobile/domain/repositories/blog_repository.dart';
import 'package:barbermanagemobile/domain/repositories/check_in_repository.dart';
import 'package:barbermanagemobile/domain/usecases/create_booking_order_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/get_all_branches_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/get_booking_service_detail_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/get_booking_services_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/get_customer_by_user_id_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/get_employees_by_branch_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/get_employees_by_date_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/get_employees_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/get_orders_by_customer_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/get_slider_images_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/get_booking_categories_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/get_booking_products_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/get_booking_product_by_id_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/get_booking_products_by_category_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/get_vips_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/get_branches_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/get_user_by_id_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/login_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/register_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/delete_booking_order_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/pay_booking_order_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/get_blogs_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/get_blog_detail_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/create_customer_promotion_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/get_service_by_id_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/get_invoice_by_order_id_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/get_employee_by_id_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/get_emp_by_user_id_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/create_check_in_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/get_check_in_history_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/update_avatar_use_case.dart';
import 'package:barbermanagemobile/presentation/providers/auth_provider.dart';
import 'package:barbermanagemobile/domain/usecases/get_all_promo_by_customer_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/get_booking_service_detail_description_use_case.dart'; // New import

final GetIt getIt = GetIt.instance;

// Register dependencies for Auth module
void _registerAuthDependencies(GetIt sl) {
  sl.registerLazySingleton<AuthRemoteDataSource>(
    () => AuthRemoteDataSourceImpl(),
    // Handles authentication API calls (login, getUser, register, updateAvatar)
  );
  sl.registerLazySingleton<AuthRepository>(
    () => AuthRepositoryImpl(sl<AuthRemoteDataSource>()),
    // Manages auth data access
  );
  sl.registerLazySingleton<LoginUseCase>(
    () => LoginUseCase(sl<AuthRepository>()),
    // Handles user login
  );
  sl.registerLazySingleton<GetUserByIDUseCase>(
    () => GetUserByIDUseCase(sl<AuthRepository>()),
    // Fetches user by ID
  );
  sl.registerLazySingleton<RegisterUseCase>(
    () => RegisterUseCase(sl<AuthRepository>()),
    // Handles user registration
  );
  sl.registerLazySingleton<UpdateAvatarUseCase>(
    () => UpdateAvatarUseCase(sl<AuthRepository>()),
    // Handles avatar updates
  );
  sl.registerFactory<AuthProvider>(
    () => AuthProvider(sl<LoginUseCase>(), sl<RegisterUseCase>()),
    // Provides auth state management
  );
}

// Register dependencies for Customer module
void _registerCustomerDependencies(GetIt sl) {
  sl.registerLazySingleton<CustomerRemoteDataSource>(
    () => CustomerRemoteDataSourceImpl(),
    // Handles customer API calls
  );
  sl.registerLazySingleton<CustomerRepository>(
    () => CustomerRepositoryImpl(sl<CustomerRemoteDataSource>()),
    // Manages customer data access
  );
  sl.registerLazySingleton<GetCustomerByUserIDUseCase>(
    () => GetCustomerByUserIDUseCase(sl<CustomerRepository>()),
    // Fetches customer by user ID
  );
}

// Register dependencies for Booking module
void _registerBookingDependencies(GetIt sl) {
  sl.registerLazySingleton<BookingServiceRemoteDataSource>(
    () => BookingServiceRemoteDataSourceImpl(),
    // Provides API calls for booking services, including getServiceById
  );
  sl.registerLazySingleton<BookingServiceRepository>(
    () => BookingServiceRepositoryImpl(sl<BookingServiceRemoteDataSource>()),
    // Manages booking service data access
  );
  sl.registerLazySingleton<BookingOrderRemoteDataSource>(
    () => BookingOrderRemoteDataSourceImpl(),
    // Handles booking order API calls
  );
  sl.registerLazySingleton<BookingOrderRepository>(
    () => BookingOrderRepositoryImpl(sl<BookingOrderRemoteDataSource>()),
    // Manages booking order data access
  );
  sl.registerLazySingleton<GetBookingServicesUseCase>(
    () => GetBookingServicesUseCase(sl<BookingServiceRepository>()),
    // Fetches all booking services
  );
  sl.registerLazySingleton<GetBookingServiceDetailUseCase>(
    () => GetBookingServiceDetailUseCase(sl<BookingServiceRepository>()),
    // Fetches detailed service info
  );
  sl.registerLazySingleton<CreateBookingOrderUseCase>(
    () => CreateBookingOrderUseCase(sl<BookingOrderRepository>()),
    // Creates a new booking order
  );
  sl.registerLazySingleton<GetAllBranchesUseCase>(
    () => GetAllBranchesUseCase(sl<BookingServiceRepository>()),
    // Fetches all branches
  );
  sl.registerLazySingleton<GetEmployeesByBranchUseCase>(
    () => GetEmployeesByBranchUseCase(sl<BookingServiceRepository>()),
    // Fetches employees by branch
  );
  sl.registerLazySingleton<GetEmployeesByDateUseCase>(
    () => GetEmployeesByDateUseCase(sl<BookingServiceRepository>()),
    // Fetches employees by date
  );
  sl.registerLazySingleton<GetOrdersByCustomerUseCase>(
    () => GetOrdersByCustomerUseCase(sl<BookingOrderRepository>()),
    // Fetches customer orders
  );
  sl.registerLazySingleton<DeleteBookingOrderUseCase>(
    () => DeleteBookingOrderUseCase(sl<BookingOrderRepository>()),
    // Deletes a booking order
  );
  sl.registerLazySingleton<PayBookingOrderUseCase>(
    () => PayBookingOrderUseCase(sl<BookingOrderRepository>()),
    // Processes payment for a booking order
  );
  sl.registerLazySingleton<GetServiceByIdUseCase>(
    () => GetServiceByIdUseCase(sl<BookingServiceRepository>()),
    // Fetches a specific service by its ID
  );
  sl.registerLazySingleton<GetInvoiceByOrderIdUseCase>(
    () => GetInvoiceByOrderIdUseCase(sl<BookingServiceRepository>()),
    // Fetches invoice by order ID
  );
  sl.registerLazySingleton<GetBookingServiceDetailDescriptionUseCase>(
    // New registration
    () => GetBookingServiceDetailDescriptionUseCase(
        sl<BookingServiceRepository>()),
    // Fetches detailed service descriptions
  );
}

// Register dependencies for Employee module
void _registerEmployeeDependencies(GetIt sl) {
  sl.registerLazySingleton<EmployeeRemoteDataSource>(
    () => EmployeeRemoteDataSourceImpl(),
    // Handles employee API calls
  );
  sl.registerLazySingleton<EmployeeRepository>(
    () => EmployeeRepositoryImpl(sl<EmployeeRemoteDataSource>()),
    // Manages employee data access
  );
  sl.registerLazySingleton<GetEmployeesUseCase>(
    () => GetEmployeesUseCase(sl<EmployeeRepository>()),
    // Fetches all employees
  );
  sl.registerLazySingleton<GetEmployeeByIdUseCase>(
    () => GetEmployeeByIdUseCase(sl<EmployeeRepository>()),
    // Fetches employee by empID
  );
  sl.registerLazySingleton<GetEmpByUserIDUseCase>(
    () => GetEmpByUserIDUseCase(sl<EmployeeRepository>()),
    // Fetches employee by userID
  );
}

// Register dependencies for Slider module
void _registerSliderDependencies(GetIt sl) {
  sl.registerLazySingleton<SliderRemoteDataSource>(
    () => SliderRemoteDataSourceImpl(),
    // Handles slider image API calls
  );
  sl.registerLazySingleton<SliderRepository>(
    () => SliderRepositoryImpl(sl<SliderRemoteDataSource>()),
    // Manages slider data access
  );
  sl.registerLazySingleton<GetSliderImagesUseCase>(
    () => GetSliderImagesUseCase(sl<SliderRepository>()),
    // Fetches slider images
  );
  sl.registerLazySingleton<GetCommitmentImageUseCase>(
      () => GetCommitmentImageUseCase(sl<SliderRepository>()));
}

// Register dependencies for Booking Category module
void _registerBookingCategoryDependencies(GetIt sl) {
  sl.registerLazySingleton<BookingCategoryRemoteDataSource>(
    () => BookingCategoryRemoteDataSourceImpl(),
    // Handles booking category API calls
  );
  sl.registerLazySingleton<BookingCategoryRepository>(
    () => BookingCategoryRepositoryImpl(sl<BookingCategoryRemoteDataSource>()),
    // Manages booking category data access
  );
  sl.registerLazySingleton<GetBookingCategoriesUseCase>(
    () => GetBookingCategoriesUseCase(sl<BookingCategoryRepository>()),
    // Fetches booking categories
  );
}

// Register dependencies for Booking Product module
void _registerBookingProductDependencies(GetIt sl) {
  sl.registerLazySingleton<BookingProductRemoteDataSource>(
    () => BookingProductRemoteDataSourceImpl(),
    // Handles booking product API calls
  );
  sl.registerLazySingleton<BookingProductRepository>(
    () => BookingProductRepositoryImpl(sl<BookingProductRemoteDataSource>()),
    // Manages booking product data access
  );
  sl.registerLazySingleton<GetBookingProductsUseCase>(
    () => GetBookingProductsUseCase(sl<BookingProductRepository>()),
    // Fetches booking products
  );
  sl.registerLazySingleton<GetBookingProductByIdUseCase>(
    () => GetBookingProductByIdUseCase(sl<BookingProductRepository>()),
    // Fetches a specific booking product by ID
  );
  sl.registerLazySingleton<GetBookingProductsByCategoryUseCase>(
    () => GetBookingProductsByCategoryUseCase(sl<BookingProductRepository>()),
    // Fetches booking products by category
  );
}

// Register dependencies for Promotion module
void _registerPromotionDependencies(GetIt sl) {
  sl.registerLazySingleton<PromotionRemoteDataSource>(
    () => PromotionRemoteDataSourceImpl(),
    // Handles promotion API calls
  );
  sl.registerLazySingleton<PromotionRepository>(
    () => PromotionRepositoryImpl(sl<PromotionRemoteDataSource>()),
    // Manages promotion data access
  );
  sl.registerLazySingleton<GetAllPromoByCustomerUseCase>(
    () => GetAllPromoByCustomerUseCase(sl<PromotionRepository>()),
    // Fetches all promotions by customer
  );
  sl.registerLazySingleton<CreateCustomerPromotionUseCase>(
    () => CreateCustomerPromotionUseCase(sl<PromotionRepository>()),
    // Creates a customer promotion
  );
}

// Register dependencies for VIP module
void _registerVipDependencies(GetIt sl) {
  sl.registerLazySingleton<VipRemoteDataSource>(
    () => VipRemoteDataSourceImpl(),
    // Handles VIP API calls
  );
  sl.registerLazySingleton<VipRepository>(
    () => VipRepositoryImpl(sl<VipRemoteDataSource>()),
    // Manages VIP data access
  );
  sl.registerLazySingleton<GetVipsUseCase>(
    () => GetVipsUseCase(sl<VipRepository>()),
    // Fetches VIP data
  );
}

// Register dependencies for Branch module
void _registerBranchDependencies(GetIt sl) {
  sl.registerLazySingleton<BranchRemoteDataSource>(
    () => BranchRemoteDataSourceImpl(),
    // Handles branch API calls
  );
  sl.registerLazySingleton<BranchRepository>(
    () => BranchRepositoryImpl(sl<BranchRemoteDataSource>()),
    // Manages branch data access
  );
  sl.registerLazySingleton<GetBranchesUseCase>(
    () => GetBranchesUseCase(sl<BranchRepository>()),
    // Fetches branches
  );
}

// Register dependencies for Blog module
void _registerBlogDependencies(GetIt sl) {
  sl.registerLazySingleton<BlogRemoteDataSource>(
    () => BlogRemoteDataSourceImpl(),
    // Handles blog API calls
  );
  sl.registerLazySingleton<BlogRepository>(
    () => BlogRepositoryImpl(sl<BlogRemoteDataSource>()),
    // Manages blog data access
  );
  sl.registerLazySingleton<GetBlogsUseCase>(
    () => GetBlogsUseCase(sl<BlogRepository>()),
    // Fetches blogs
  );
  sl.registerLazySingleton<GetBlogDetailUseCase>(
    () => GetBlogDetailUseCase(sl<BlogRepository>()),
    // Fetches blog details
  );
}

// Register dependencies for Check-In module
void _registerCheckInDependencies(GetIt sl) {
  sl.registerLazySingleton<CheckInRemoteDataSource>(
    () => CheckInRemoteDataSourceImpl(),
    // Handles check-in API calls
  );
  sl.registerLazySingleton<CheckInRepository>(
    () => CheckInRepositoryImpl(sl<CheckInRemoteDataSource>()),
    // Manages check-in data access
  );
  sl.registerLazySingleton<CreateCheckInUseCase>(
    () => CreateCheckInUseCase(sl<CheckInRepository>()),
    // Creates a new check-in
  );
  sl.registerLazySingleton<GetCheckInHistoryUseCase>(
    () => GetCheckInHistoryUseCase(sl<CheckInRepository>()),
    // Fetches check-in history
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
  _registerBookingProductDependencies(sl);
  _registerPromotionDependencies(sl);
  _registerVipDependencies(sl);
  _registerBranchDependencies(sl);
  _registerBlogDependencies(sl);
  _registerCheckInDependencies(sl);
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
  if (!getIt.isRegistered<BookingProductRepository>()) {
    throw Exception('BookingProductRepository is not registered');
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
  if (!getIt.isRegistered<BlogRepository>()) {
    throw Exception('BlogRepository is not registered');
  }
  if (!getIt.isRegistered<CheckInRepository>()) {
    throw Exception('CheckInRepository is not registered');
  }
  if (!getIt.isRegistered<LoginUseCase>()) {
    throw Exception('LoginUseCase is not registered');
  }
  if (!getIt.isRegistered<GetUserByIDUseCase>()) {
    throw Exception('GetUserByIDUseCase is not registered');
  }
  if (!getIt.isRegistered<RegisterUseCase>()) {
    throw Exception('RegisterUseCase is not registered');
  }
  if (!getIt.isRegistered<UpdateAvatarUseCase>()) {
    throw Exception('UpdateAvatarUseCase is not registered');
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
  if (!getIt.isRegistered<GetAllPromoByCustomerUseCase>()) {
    throw Exception('GetAllPromoByCustomerUseCase is not registered');
  }
  if (!getIt.isRegistered<CreateCustomerPromotionUseCase>()) {
    throw Exception('CreateCustomerPromotionUseCase is not registered');
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
  if (!getIt.isRegistered<GetBlogsUseCase>()) {
    throw Exception('GetBlogsUseCase is not registered');
  }
  if (!getIt.isRegistered<GetBlogDetailUseCase>()) {
    throw Exception('GetBlogDetailUseCase is not registered');
  }
  if (!getIt.isRegistered<GetBookingProductsUseCase>()) {
    throw Exception('GetBookingProductsUseCase is not registered');
  }
  if (!getIt.isRegistered<GetBookingProductByIdUseCase>()) {
    throw Exception('GetBookingProductByIdUseCase is not registered');
  }
  if (!getIt.isRegistered<GetBookingProductsByCategoryUseCase>()) {
    throw Exception('GetBookingProductsByCategoryUseCase is not registered');
  }
  if (!getIt.isRegistered<GetServiceByIdUseCase>()) {
    throw Exception('GetServiceByIdUseCase is not registered');
  }
  if (!getIt.isRegistered<GetInvoiceByOrderIdUseCase>()) {
    throw Exception('GetInvoiceByOrderIdUseCase is not registered');
  }
  if (!getIt.isRegistered<GetEmployeeByIdUseCase>()) {
    throw Exception('GetEmployeeByIdUseCase is not registered');
  }
  if (!getIt.isRegistered<GetEmpByUserIDUseCase>()) {
    throw Exception('GetEmpByUserIDUseCase is not registered');
  }
  if (!getIt.isRegistered<CreateCheckInUseCase>()) {
    throw Exception('CreateCheckInUseCase is not registered');
  }
  if (!getIt.isRegistered<GetCheckInHistoryUseCase>()) {
    throw Exception('GetCheckInHistoryUseCase is not registered');
  }
  if (!getIt.isRegistered<GetBookingServiceDetailDescriptionUseCase>()) {
    // New verification
    throw Exception(
        'GetBookingServiceDetailDescriptionUseCase is not registered');
  }
}
