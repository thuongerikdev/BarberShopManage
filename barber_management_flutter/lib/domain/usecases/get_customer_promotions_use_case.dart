import 'package:barbermanagemobile/domain/entities/customer_promotion.dart';
import 'package:barbermanagemobile/domain/repositories/customer_promotion_repository.dart';

class GetCustomerPromotionsUseCase {
  final CustomerPromotionRepository repository;

  GetCustomerPromotionsUseCase(this.repository);

  Future<List<CustomerPromotion>> call(int customerId) async {
    return await repository.getCustomerPromotions(customerId);
  }
}