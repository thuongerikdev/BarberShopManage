import 'package:barbermanagemobile/domain/repositories/customer_promotion_repository.dart';

class CreateCustomerPromotionUseCase {
  final CustomerPromotionRepository repository;

  CreateCustomerPromotionUseCase(this.repository);

  Future<void> call(int customerId, int promoId, String status) async {
    await repository.createCustomerPromotion(customerId, promoId, status);
  }
}